using AutoFuzzer.BussinessLogic.Services;
using AutoFuzzer.Domain.Extensions;
using AutoFuzzer.Domain.Models;
using AutoFuzzer.WorkerService.Resources;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace AutoFuzzer.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private const int _timeInMs = 10000;
        static private readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private readonly string _crashLogPath = Path.Combine(_baseDirectory, "CrashLogs");
        private readonly string _crashLogsFile = Path.Combine(_baseDirectory, "CrashLogs", "crashes.log");
        private readonly string _newDirectoryCrashPath = Path.Combine("CrashLogs", "Crashes");
        private readonly string _directoryCrashPath = Path.Combine(_baseDirectory, "Findings");
        private ContainerService _containerService;
        private HashSet<string> _processedFiles;

        public Worker(ContainerService containerService, ILogger<Worker> logger)
        {
            _containerService = containerService;
            _processedFiles = new HashSet<string>();
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DeleteFuzzerWSDTOAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    CopyFindingsFromContainer();
                    var fuzzerWSDTO = await TryGetFuzzerWSDTOAsync();

                    if (fuzzerWSDTO != null)
                    {
                        if (!Directory.Exists(_directoryCrashPath))
                            Directory.CreateDirectory(_directoryCrashPath);

                        var directory = new DirectoryInfo(_directoryCrashPath);
                        var innerDirectories = directory.GetDirectories();
                        Scan(innerDirectories, fuzzerWSDTO.TestDllPath, fuzzerWSDTO.ReflectionProject);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                await Task.Delay(_timeInMs, stoppingToken);
            }
        }

        private async Task DeleteFuzzerWSDTOAsync()
        {
            using (var client = new HttpClient())
            {
                await client.GetAsync("http://localhost:5000/Fuzzer/DeleteFuzzerWSDTO");
            };
        }
        private async Task<FuzzerWSDTO> TryGetFuzzerWSDTOAsync()
        {
            using (var client = new HttpClient())
            {
                using (var result = await client.GetAsync("http://localhost:5000/Fuzzer/GetFuzzerWSDTO"))
                {
                    var json = await result.Content.ReadAsStringAsync();
                    if (!String.IsNullOrEmpty(json))
                    {
                        return JsonSerializer.Deserialize<FuzzerWSDTO>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                }
            }

            return null;
        }

        private void Scan(DirectoryInfo[] innerDirectories, string testDllPath, ReflectionProject reflectionProject)
        {
            foreach (var innerDirectory in innerDirectories)
            {
                foreach (var crashDirectory in innerDirectory.GetDirectories())
                {
                    foreach (var reflectionClass in reflectionProject.ReflectionClasses)
                    {
                        foreach (var reflectionMethod in reflectionClass.ReflectionMethods)
                        {
                            var snakeCaseMethodName = reflectionMethod.MethodName.ToSnakeCase();
                            if (!innerDirectory.Name.Contains(snakeCaseMethodName))
                                continue;

                            foreach (var file in crashDirectory.GetFiles())
                            {
                                if (!file.FullName.Contains("id") || isProcessed(innerDirectory.Name, file.Name))
                                    continue;

                                var data = File.ReadAllText(file.FullName);

                                try
                                {
                                    InvokeMethod(testDllPath, reflectionClass.ClassName, reflectionMethod.MethodName, data);
                                }
                                catch (TimeoutException ex)
                                {
                                    var message = $"{DateTime.Now}" +
                                        $"\nInput File {file.Name} with data: {data}" +
                                        $"\nException {reflectionClass.ClassName}.{reflectionMethod.MethodName} Details:" +
                                        $"\nException Message: {ex.Message}\n\n";
                                    var logName = string.Concat(reflectionClass.ClassName, ".", reflectionMethod.MethodName, ".log");
                                    SaveToLog(logName, message);
                                }
                                catch (Exception ex)
                                {
                                    var message = $"{DateTime.Now}" +
                                        $"\nInput File {file.Name} with data: {data}" +
                                        $"\nException {reflectionClass.ClassName}.{reflectionMethod.MethodName} Details:" +
                                        $"\nException Message: {ex.InnerException?.Message}" +
                                        $"\nException StackTrace: {ex.InnerException?.StackTrace}\n\n";
                                    var logName = string.Concat(reflectionClass.ClassName, ".", reflectionMethod.MethodName, ".log");
                                    SaveToLog(logName,message);
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool isProcessed(string dictionaryName, string file)
        {
            var fullName = String.Join('.', dictionaryName, file);
            
            if (_processedFiles.Contains(fullName))
                return true;
            
            _processedFiles.Add(fullName);
            return false;
        }

        private void InvokeMethod(string testDllPath, string testClassName, string testMethodName, string data)
        {
            Assembly asm = Assembly.LoadFrom(testDllPath);
            var targetDllName = testDllPath.Split(new char[] { '/', '\\' }).Last();
            Type t = asm.GetType($"{targetDllName.Substring(0, targetDllName.Length - 4)}.{testClassName}");

            var methodInfo = t.GetMethod(testMethodName, new Type[] { typeof(string) });
            if (methodInfo == null)
            {
                throw new Exception("No such method exists.");
            }

            var o = Activator.CreateInstance(t, null);
            object[] parameters = new object[1];
            parameters[0] = data;

            if (!Task.Run(() => _ = methodInfo.Invoke(o, parameters)).Wait(TimeSpan.FromSeconds(30)))
                throw new TimeoutException("Method exceeded execution time, possible loop error");
        }

        private void SaveToLog(string crashLogName, string message)
        {
            var crashLogFile = Path.Combine(_crashLogPath, crashLogName);
            var directoryPath = Path.GetDirectoryName(crashLogFile);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            if (!File.Exists(crashLogFile))
                using (StreamWriter sw = File.CreateText(crashLogFile))
                    sw.WriteLine(message);
            else
                using (StreamWriter sw = File.AppendText(crashLogFile))
                    sw.WriteLine(message);
        }

        private void CopyFindingsFromContainer()
        {
            var command = "/c docker ps --format \"{{.Names}}\"";
            var cmdsi = new ProcessStartInfo("cmd.exe");
            foreach (string cName in _containerService.GetRunningContainerNames())
            {
                var containerPath = Path.Combine(_directoryCrashPath, cName);
                command = $"/c mkdir {containerPath}";
                cmdsi.Arguments = command;
                using (var cmd = Process.Start(cmdsi)) { }

                command = $"/c docker cp {cName}:/app/FuzzerApp/findings/default/crashes {containerPath}";
                cmdsi.Arguments = command;
                using (var cmd = Process.Start(cmdsi)) { }
            }
        }
    }
}
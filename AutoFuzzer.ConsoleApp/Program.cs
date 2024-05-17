using AutoFuzzer.BussinessLogic.Services;
using AutoFuzzer.Domain.Models;
using System.Diagnostics;
using System.Reflection;
using YamlDotNet.Core.Tokens;

static class Program
{
    private static readonly string _crashLogsFile = Path.Combine("CrashesLogs", "crashes.log");
    private static string _findingsDir = Path.Combine("..", "..", "..", "..", "Findings");

    static void Main(string[] args)
    {
        /*СopyFindingsFromContainer();*/

        //var testDllPath = "C:\\Users\\Valugs\\Desktop\\Programming\\C#DotNet\\AutoFuzzer\\Calculator.FuzzerTests\\bin\\Debug\\net6.0\\Calculator.FuzzerTests.dll";
        //var reflectionService = new ReflectionService();
        //var reflectionProject = reflectionService.ParseProject(testDllPath);

        //var directoryPath = Path.Combine("..", "..", "..", "..", "Findings");
        //var directory = new DirectoryInfo(directoryPath);
        //var innerDirectories = directory.GetDirectories();
        //Scan(innerDirectories, testDllPath, reflectionProject);
    }

    private static void CopyFindingsFromContainer()
    {

        var command = "/c docker ps --format \"{{.Names}}\"";
        var cmdsi = new ProcessStartInfo("cmd.exe");
        cmdsi.Arguments = command;
        cmdsi.RedirectStandardOutput = true;
        cmdsi.UseShellExecute = false;
        string output;
        using (var cmd = Process.Start(cmdsi)) {
            output = cmd.StandardOutput.ReadToEnd();
        }
        
        output = output.Remove(output.Length - 1);

        string[] containersNames = output.Split('\n');

        foreach (string cName in containersNames)
        {
            var containerPath = Path.Combine(_findingsDir, cName);
            command = $"/c mkdir {containerPath}";
            cmdsi.Arguments = command;
            using (var cmd = Process.Start(cmdsi)) { }

            command = $"/c docker cp {cName}:/app/FuzzerApp/findings/default/crashes {containerPath}";
            cmdsi.Arguments = command;
            using (var cmd = Process.Start(cmdsi)) { }
        }
    }

    private static void Scan(DirectoryInfo[] innerDirectories, string testDllPath, ReflectionProject reflectionProject)
    {
        foreach (var innerDirectory in innerDirectories)
        {
            foreach (var reflectionClass in reflectionProject.ReflectionClasses)
            {
                foreach (var reflectionMethod in reflectionClass.ReflectionMethods)
                {
                    if (IsHasAllCharacters(innerDirectory.Name, reflectionMethod.MethodName))
                    {
                        foreach (var file in innerDirectory.GetFiles())
                        {
                            var data = File.ReadAllText(file.FullName);

                            try
                            {
                                InvokeMethod(testDllPath, reflectionClass.ClassName, reflectionMethod.MethodName, data);
                            }
                            catch (TimeoutException ex)
                            {
                                var message = $"{DateTime.Now}" +
                                    $"\nInput data: {data}" +
                                    $"\nException {reflectionClass.ClassName}.{reflectionMethod.MethodName} Details:" +
                                    $"\nException Message: {ex.Message}\n\n";

                                SaveToLog(message);
                            }
                            catch (Exception ex)
                            {
                                var message = $"{DateTime.Now}" +
                                    $"\nInput data: {data}" +
                                    $"\nException {reflectionClass.ClassName}.{reflectionMethod.MethodName} Details:" +
                                    $"\nException Message: {ex.InnerException?.Message}" +
                                    $"\nException StackTrace: {ex.InnerException?.StackTrace}\n\n";

                                SaveToLog(message);
                            }
                        }
                    }
                }
            }
        }
    }

    private static bool IsHasAllCharacters(string sourceString,  string targetString)
    {
        sourceString = sourceString.ToLower();
        targetString = targetString.ToLower();

        return targetString.All(t => sourceString.Contains(t)) ? true : false;
    }

    private static void InvokeMethod(string testDllPath, string testClassName, string testMethodName, string data)
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

    private static void SaveToLog(string message)
    {
        var directoryPath = Path.GetDirectoryName(_crashLogsFile);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        if (!File.Exists(_crashLogsFile))
            using (StreamWriter sw = File.CreateText(_crashLogsFile))
                sw.WriteLine(message);
        else
            using (StreamWriter sw = File.AppendText(_crashLogsFile))
                sw.WriteLine(message);
    }
}
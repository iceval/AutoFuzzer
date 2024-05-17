

using AutoFuzzer.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AutoFuzzer.BussinessLogic.Services
{
    public class LogService
    {
        private readonly string _DirectoryCrashPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CrashLogs");
        private readonly ILogger _logger;

        public LogService(ILogger<LogService> logger)
        {
            _logger = logger;
        }


        public List<string> GetLogNames()
        {
            _logger.LogInformation($"Get log names");
            var logNames = new List<string>();
            if (Directory.Exists(_DirectoryCrashPath))
                logNames = Directory.GetFiles(_DirectoryCrashPath).Select(file => Path.GetFileName(file)).ToList();
            _logger.LogInformation($"Return log names");
            return logNames;
        }

        public LogFile GetLog(string filename)
        {
            _logger.LogInformation($"Get log");
            if (filename == null)
                throw new Exception("filename not present");

            var path = Path.Combine(_DirectoryCrashPath, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            _logger.LogInformation($"Return log");
            return new LogFile() 
            {
                Memory = memory, 
                Filename = Path.GetFileName(path)
            };
        }
    }
}

using AutoFuzzer.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace AutoFuzzer.BussinessLogic.Services
{
    public class ReflectionService
    {
        private readonly string testsTemplate = "Tests";
        private readonly List<string> reservedMethods = new List<string>() { "GetType", "ToString", "Equals", "GetHashCode" };
        private readonly ILogger _logger;

        public ReflectionService(ILogger<ReflectionService> logger)
        {
            _logger = logger;
        }

        public ReflectionProject ParseProject(string dllPath)
        {
            if (!File.Exists(dllPath))
            {
                _logger.LogInformation($"Not exists dll path");
                return null;
            }

            _logger.LogInformation($"Start parsing");
            var assembly = Assembly.LoadFile(dllPath);
            var reflectionProject = new ReflectionProject();
            reflectionProject.ProjectName = Path.GetFileName(dllPath); // Взять с конца название
            reflectionProject.ReflectionClasses = ParseClasses(assembly);
            _logger.LogInformation($"Finish parsing");

            FillDictionaryPaths(dllPath, reflectionProject);

            return reflectionProject;
        }

        private List<ReflectionClass> ParseClasses(Assembly assembly)
        {
            var reflectionClasses = new List<ReflectionClass>();
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Name.Contains(testsTemplate)) {
                    reflectionClasses.Add(new ReflectionClass()
                    {
                        ClassName = type.Name,
                        ReflectionMethods = ParseMethods(type)
                    });
                }
            }
            return reflectionClasses;
        }

        private List<ReflectionMethod> ParseMethods(Type type)
        {
            var reflectionMethods = new List<ReflectionMethod>();
            foreach (MethodInfo method in type.GetMethods())
            {
                if (reservedMethods.Contains(method.Name))
                    continue;

                reflectionMethods.Add(new ReflectionMethod()
                {
                    MethodName = method.Name
                });
            }

            return reflectionMethods;
        }

        private void FillDictionaryPaths(string dllPath, ReflectionProject reflectionProject)
        {
            var dllDirectoryPath = Path.GetDirectoryName(dllPath);
            var dictionaryPath = Path.Combine(dllDirectoryPath, "Dictionaries");
            if (!Directory.Exists(dictionaryPath)) 
            {
                return;
            }

            foreach (var reflectionClass in reflectionProject.ReflectionClasses)
            {
                foreach (var reflectionMethod in reflectionClass.ReflectionMethods)
                {
                    var dictionaryMethodPath = Path.Combine(dictionaryPath, reflectionMethod.MethodName);
                    var di = new DirectoryInfo(dictionaryMethodPath);

                    if (Directory.Exists(dictionaryMethodPath)) { 
                    reflectionMethod.DictionaryName = di.GetFiles()
                        .Select(fi => fi.Name)
                        .FirstOrDefault();
                    }
                }
            }
        }
    }
}

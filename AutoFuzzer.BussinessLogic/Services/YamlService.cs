using AutoFuzzer.Domain.Models;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using AutoFuzzer.Domain.Extensions;
using Microsoft.Extensions.Logging;

namespace AutoFuzzer.BussinessLogic.Services
{
    public class YamlService
    {
        static private readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private readonly ILogger _logger;

        public YamlService(ILogger<YamlService> logger)
        {
            _logger = logger;
        }

        public Dictionary<string, YamlContainer> CreateYamlContainers(string dllName, string testDllName, ReflectionProject reflectionProject)
        {
            _logger.LogInformation($"Create yaml containers");
            var yamlContainers = new Dictionary<string, YamlContainer>();
            foreach (var reflectionClass in reflectionProject.ReflectionClasses)
            {
                foreach (var reflectionMethod in reflectionClass.ReflectionMethods)
                {
                    if (!reflectionMethod.IsSelected)
                        continue;

                    var containerName = reflectionMethod.MethodName.ToSnakeCase();
                    YamlContainer container;
                    if (reflectionMethod.DictionaryName != null)
                    {
                        container = new YamlContainer
                        {
                            Build = ".",
                            Environment = new List<string>
                            {
                                $"TARGET_DICTIONARY={reflectionMethod.DictionaryName}",
                                $"TARGET_DLL={dllName}",
                                $"TARGET_TEST_DLL={testDllName}",
                                $"TARGET_TEST_CLASS_NAME={reflectionClass.ClassName}",
                                $"TARGET_TEST_METHOD_NAME={reflectionMethod.MethodName}"
                            }
                        };
                    }
                    else 
                    {
                        container = new YamlContainer
                        {
                            Build = ".",
                            Environment = new List<string>
                            {
                                $"TARGET_DLL={dllName}",
                                $"TARGET_TEST_DLL={testDllName}",
                                $"TARGET_TEST_CLASS_NAME={reflectionClass.ClassName}",
                                $"TARGET_TEST_METHOD_NAME={reflectionMethod.MethodName}"
                            }
                        };
                    }
                    

                    yamlContainers.Add(containerName, container);
                }
            }

            _logger.LogInformation($"Return yaml containers");
            return yamlContainers;
        }

        public void CreateOrRewriteDockerCompose(Dictionary<string, YamlContainer> yamlServices)
        {
            var yamlRoot = new YamlRoot
            {
                Services = yamlServices
            };

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var yaml = serializer.Serialize(yamlRoot);
            yaml = "version: '3.9'\r\n" + yaml;

            var dockerComposePath = Path.Combine(_baseDirectory, "docker-compose.yml");

            File.WriteAllText(dockerComposePath, yaml.Replace("\r\n", "\n")); // Convert CR LF (Windows) to LF (Unix)
        }
     }
}

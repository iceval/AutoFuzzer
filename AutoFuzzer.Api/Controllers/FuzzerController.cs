using AutoFuzzer.Api.Resources;
using AutoFuzzer.BussinessLogic.Services;
using AutoFuzzer.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace AutoFuzzer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FuzzerController : ControllerBase
    {
        private readonly ILogger<FuzzerController> _logger;
        static private readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static private readonly string _dockerComposePath = Path.Combine(_baseDirectory, "docker-compose.yml");
        private readonly ReflectionService _reflectionService;
        private readonly YamlService _yamlService;
        private readonly FuzzerAppService _fuzzerAppService;
        private readonly string _fuzzerWSDTOFile = Path.Combine(_baseDirectory,"fuzzerWSDTO.json");

        public FuzzerController(ILogger<FuzzerController> logger, ReflectionService reflectionService, YamlService yamlService, FuzzerAppService fuzzerAppService)
        {
            _logger = logger;
            _reflectionService = reflectionService;
            _yamlService = yamlService;
            _fuzzerAppService = fuzzerAppService;
        }

        [HttpPost("GetReflectionProject")]
        public ReflectionProject PostReflectionProject(TestDllPathDTO testDllPathDTO)
        {
            return _reflectionService.ParseProject(testDllPathDTO.TestDllPath);
        }

        [HttpGet("GetFuzzerWSDTO")]
        public FuzzerWSDTO GetFuzzerWSDTO()
        {
            return GetFuzzerWSDTOFromJson();
        }

        [HttpGet("DeleteFuzzerWSDTO")]
        public void DeleteFuzzerWSDTO()
        {
            if (System.IO.File.Exists(_fuzzerWSDTOFile))
                System.IO.File.Delete(_fuzzerWSDTOFile);
        }

        [HttpPost("Run")]
        public void Run(FuzzerRunDTO fuzzerRunDTO)
        {
            string testDllName = Path.GetFileName(fuzzerRunDTO.TestDllPath);
            string testDllProjectPath = Path.GetDirectoryName(fuzzerRunDTO.TestDllPath);

            CreateFuzzerWSDTO(fuzzerRunDTO.TestDllPath, fuzzerRunDTO.ReflectionProject);

            string fuzzerAppMainPath = _fuzzerAppService.ClearAndCopyFolder(testDllProjectPath);
            string fuzzerAppPath = Directory.GetParent(fuzzerAppMainPath).FullName;

            var dllName = Path.GetFileName(fuzzerRunDTO.DllPath);
            var yamlContainers = _yamlService.CreateYamlContainers(dllName, testDllName, fuzzerRunDTO.ReflectionProject);
            _yamlService.CreateOrRewriteDockerCompose(yamlContainers);

            var testDllPath = Path.Combine(fuzzerAppMainPath, testDllName);
            var fuzzerAppCsprojPath = Path.Combine(fuzzerAppPath, "FuzzerApp.csproj");
            _fuzzerAppService.ChangeDllProperty(fuzzerAppCsprojPath, testDllPath);
            
            string cmdCommand = $"/C docker-compose -f {_dockerComposePath} up -d";
            Process.Start("CMD.exe", cmdCommand);
        }

        private void CreateFuzzerWSDTO(string testDllPath, ReflectionProject reflectionProject)
        {
            var fuzzerWSDTO = new FuzzerWSDTO
            {
                TestDllPath = testDllPath,
                ReflectionProject = reflectionProject
            };
            var jsonSerializer = JsonSerializer.Serialize<FuzzerWSDTO>(fuzzerWSDTO);
            using (FileStream fs = new FileStream(_fuzzerWSDTOFile, FileMode.OpenOrCreate))
            {
                byte[] buffer = Encoding.Default.GetBytes(jsonSerializer);
                // ������ ������� ������ � ����
                fs.Write(buffer, 0, buffer.Length);
            }
        }

        private FuzzerWSDTO GetFuzzerWSDTOFromJson()
        {
            if (!System.IO.File.Exists(_fuzzerWSDTOFile))
                return null;

            var fuzzerWSDTO = JsonSerializer.Deserialize<FuzzerWSDTO>(System.IO.File.ReadAllText(_fuzzerWSDTOFile));

            return fuzzerWSDTO;
        }
    }
}
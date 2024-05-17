using AutoFuzzer.Domain.Models;

namespace AutoFuzzer.Api.Resources
{
    public class FuzzerRunDTO
    {
        public string TestDllPath { get; set; } // Test dll path
        public string DllPath { get; set; } // Dll path
        public ReflectionProject ReflectionProject { get; set; } // test dll assembly with labels
    }
}

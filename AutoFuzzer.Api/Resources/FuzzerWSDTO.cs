using AutoFuzzer.Domain.Models;

namespace AutoFuzzer.Api.Resources
{
    public class FuzzerWSDTO
    {
        public string TestDllPath { get; set; }
        public ReflectionProject ReflectionProject { get; set; }
    }
}

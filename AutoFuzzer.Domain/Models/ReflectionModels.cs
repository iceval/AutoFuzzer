namespace AutoFuzzer.Domain.Models
{
    public class ReflectionProject
    {
        public string ProjectName { get; set; }
        public List<ReflectionClass> ReflectionClasses { get; set; }
    }

    public class ReflectionClass
    {
        public string ClassName { get; set; }
        public List<ReflectionMethod> ReflectionMethods { get; set; }
    }

    public class ReflectionMethod
    {
        public string MethodName { get; set; }
        public bool IsSelected { get; set; }
        public string? DictionaryName { get; set; }
    }
}
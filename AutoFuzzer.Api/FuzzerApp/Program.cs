namespace FuzzerApp;

using SharpFuzz;
using System.Reflection;

static partial class Program
{
    private static string _targetTestDll;
    private static string _targetTestClassName;
    private static string _targetTestMethodName;

    static void Main(string[] args)
    {
        FuzzerTests_AllMethods(args);
    }

    private static void Run(Action<string> action) => Fuzzer.OutOfProcess.Run(action);

    private static void FuzzerTests_AllMethods(string[] args)
    {
        _targetTestDll = args[0];
        _targetTestClassName = args[1];
        _targetTestMethodName = args[2];

        Run(FuzzerTests_Method);
    }

    public static void FuzzerTests_Method(string data)
    {
        string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _targetTestDll);
        Assembly asm = Assembly.LoadFrom(dllPath);
        var targetDllName = _targetTestDll.Split(new char[] { '/', '\\' }).Last();
        Type t = asm.GetType($"{targetDllName.Substring(0, targetDllName.Length - 4)}.{_targetTestClassName}");

        var methodInfo = t.GetMethod(_targetTestMethodName, new Type[] { typeof(string) });
        if (methodInfo == null)
        {
            throw new Exception("No such method exists.");
        }

        var o = Activator.CreateInstance(t, null);
        object[] parameters = new object[1];
        parameters[0] = data;
        _ = methodInfo.Invoke(o, parameters);
    }
}

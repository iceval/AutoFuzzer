using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace AutoFuzzer.BussinessLogic.Services
{
    public class FuzzerAppService
    {
        static private readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private readonly ILogger _logger;

        public FuzzerAppService(ILogger<FuzzerAppService> logger)
        {
            _logger = logger;
        }

        public string ClearAndCopyFolder(string sourceFolderPath)
        {
            string destFolderPath = Path.Combine(_baseDirectory, "FuzzerApp", "Main");
            if (!Directory.Exists(destFolderPath))
                Directory.CreateDirectory(destFolderPath);

            _logger.LogInformation($"Clear folder");
            DirectoryInfo di = new DirectoryInfo(destFolderPath);
            foreach (FileInfo file in di.GetFiles())
                file.Delete();
            foreach (DirectoryInfo dir in di.GetDirectories())
                dir.Delete(true);

            _logger.LogInformation($"Copy folder");
            string[] filePaths = Directory.GetFiles(sourceFolderPath);
            foreach (var filename in filePaths)
            {
                string sourceFile = filename.ToString();
                string destFile = Path.Combine(destFolderPath, Path.GetFileName(sourceFile));
                File.Copy(sourceFile, destFile, true);
            }
            CloneDirectory(Path.Combine(sourceFolderPath, "TestCases"), Path.Combine(destFolderPath, "TestCases"));
            if (Directory.Exists(Path.Combine(sourceFolderPath, "Dictionaries")))
                CloneDirectory(Path.Combine(sourceFolderPath, "Dictionaries"), Path.Combine(destFolderPath, "Dictionaries"));
            
            return destFolderPath;
        }

        private static void CloneDirectory(string root, string dest)
        {
            if (!Directory.Exists(dest))
                Directory.CreateDirectory(dest);

            foreach (var directory in Directory.GetDirectories(root))
            {
                //Get the path of the new directory
                var newDirectory = Path.Combine(dest, Path.GetFileName(directory));
                //Create the directory if it doesn't already exist
                Directory.CreateDirectory(newDirectory);
                //Recursively clone the directory
                CloneDirectory(directory, newDirectory);
            }

            foreach (var file in Directory.GetFiles(root))
            {
                File.Copy(file, Path.Combine(dest, Path.GetFileName(file)));
            }
        }

        public void ChangeDllProperty(string projectPath, string testDllPath)
        {
            _logger.LogInformation($"Change dll property");
            var xml = XElement.Load(projectPath);

            var node = xml.Descendants("Reference")
                .FirstOrDefault(r => r.Attribute("Include") != null && r.Element("HintPath") != null);

            if (node == null)
                throw new Exception("Not found FuzzerApp.dll");

            node.Attribute("Include").Value = Path.GetFileNameWithoutExtension(testDllPath);
            var hintPath = node.Element("HintPath");
            var path = hintPath.Value.TrimEnd('\\').Remove(hintPath.Value.LastIndexOf('\\') + 1);
            hintPath.Value = Path.Combine(path, Path.GetFileName(testDllPath));

            xml.Save(projectPath);
        }
    }
}

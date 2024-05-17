using Microsoft.Extensions.Logging;
using System.Diagnostics;


namespace AutoFuzzer.BussinessLogic.Services
{
    public class ContainerService
    {
        private readonly ILogger _logger;


        public ContainerService(ILogger<ContainerService> logger) 
        {
            _logger = logger;
        }

        public List<string> GetRunningContainerNames() 
        {
            _logger.LogInformation($"Get running container names");
            List<string> containerNames = new List<string>();
            var command = "/c docker container ls -f status=running -a --format \"{{.Names}}\"";
            var cmdsi = new ProcessStartInfo("cmd.exe");
            cmdsi.Arguments = command;
            cmdsi.RedirectStandardOutput = true;
            cmdsi.UseShellExecute = false;
            string output;
            using (var cmd = Process.Start(cmdsi))
            {
                output = cmd.StandardOutput.ReadToEnd();
            }
            if (!String.IsNullOrEmpty(output))
            {
                output = output.Remove(output.Length - 1);

                containerNames = output.Split('\n').ToList();
            }
            _logger.LogInformation($"Return running container names");

            return containerNames;
        }

        public List<string> GetPausedContainerNames()
        {
            _logger.LogInformation($"Get paused container names");
            List<string> containerNames = new List<string>();
            var command = "/c docker container ls -f status=paused -a --format \"{{.Names}}\"";
            var cmdsi = new ProcessStartInfo("cmd.exe");
            cmdsi.Arguments = command;
            cmdsi.RedirectStandardOutput = true;
            cmdsi.UseShellExecute = false;
            string output;
            using (var cmd = Process.Start(cmdsi))
            {
                output = cmd.StandardOutput.ReadToEnd();
            }
            if (!String.IsNullOrEmpty(output))
            {
                output = output.Remove(output.Length - 1);

                containerNames = output.Split('\n').ToList();
            }
            _logger.LogInformation($"Return paused container names");

            return containerNames;
        }

        public void RunContainer(string containerName)
        {
            var command = string.Format("/c docker unpause {0}", containerName);
            var cmdsi = new ProcessStartInfo("cmd.exe");
            cmdsi.Arguments = command;
            cmdsi.RedirectStandardOutput = true;
            cmdsi.UseShellExecute = false;
            Process.Start(cmdsi);
            _logger.LogInformation($"Run container");
        }

        public void PauseContainer(string containerName)
        {
            var command = string.Format("/c docker pause {0}", containerName);
            var cmdsi = new ProcessStartInfo("cmd.exe");
            cmdsi.Arguments = command;
            cmdsi.RedirectStandardOutput = true;
            cmdsi.UseShellExecute = false;
            Process.Start(cmdsi);
            _logger.LogInformation($"Pause container");
        }

        public void DeleteContainer(string containerName)
        {
            var command = string.Format("/c docker stop {0} & docker rm {0}", containerName);
            var cmdsi = new ProcessStartInfo("cmd.exe");
            cmdsi.Arguments = command;
            cmdsi.RedirectStandardOutput = true;
            cmdsi.UseShellExecute = false;
            Process.Start(cmdsi);
            _logger.LogInformation($"Delete container");
        }

        public void RunAllContainers()
        {
            var command = string.Format("/c FOR /f \"tokens=*\" %i IN ('docker ps -q') DO docker unpause %i");
            var cmdsi = new ProcessStartInfo("cmd.exe");
            cmdsi.Arguments = command;
            cmdsi.RedirectStandardOutput = true;
            cmdsi.UseShellExecute = false;
            Process.Start(cmdsi);
            _logger.LogInformation($"Run all containers");
        }
        public void PauseAllContainers()
        {
            var command = string.Format("/c FOR /f \"tokens=*\" %i IN ('docker ps -q') DO docker pause %i");
            var cmdsi = new ProcessStartInfo("cmd.exe");
            cmdsi.Arguments = command;
            cmdsi.RedirectStandardOutput = true;
            cmdsi.UseShellExecute = false;
            Process.Start(cmdsi);
            _logger.LogInformation($"Pause all containers");
        }
        public void DeleteAllContainers() {
            var command = string.Format("/c FOR /f \"tokens=*\" %i IN ('docker ps -q') DO docker stop %i & docker rm %i");
            var cmdsi = new ProcessStartInfo("cmd.exe");
            cmdsi.Arguments = command;
            cmdsi.RedirectStandardOutput = true;
            cmdsi.UseShellExecute = false;
            Process.Start(cmdsi);
            _logger.LogInformation($"Delete all containers");
        }

    }
}

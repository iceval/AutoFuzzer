using AutoFuzzer.BussinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoFuzzer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContainerController : ControllerBase
    {
        private readonly ContainerService _containerService;

        public ContainerController(ContainerService containerService)
        {
            _containerService = containerService;
        }

        [HttpGet("GetRunningContainerNames")]
        public List<string> GetContainerNames()
        {
            return _containerService.GetRunningContainerNames();
        }

        [HttpGet("GetPausedContainerNames")]
        public List<string> GetPausedContainerNames()
        {
            return _containerService.GetPausedContainerNames();
        }

        [HttpPost("RunContainer")]
        public void EnableContainer(string containerName)
        {
            _containerService.RunContainer(containerName);
        }

        [HttpPost("PauseContainer")]
        public void DisableContainer(string containerName)
        {
            _containerService.PauseContainer(containerName);
        }

        [HttpPost("DeleteContainer")]
        public void DeleteContainer(string containerName)
        {
            _containerService.DeleteContainer(containerName);
        }

        [HttpPost("RunAllContainers")]
        public void DisableAllContainers()
        {
            _containerService.RunAllContainers();
        }

        [HttpPost("PauseAllContainers")]
        public void PauseAllContainers()
        {
            _containerService.PauseAllContainers();
        }

        [HttpPost("DeleteAllContainers")]
        public void DeleteAllContainers()
        {
            _containerService.DeleteAllContainers();
        }
    }
}

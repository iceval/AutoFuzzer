using AutoFuzzer.BussinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using AutoFuzzer.Domain.Models;

namespace AutoFuzzer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogController : ControllerBase
    {
        private readonly LogService _logService;

        public LogController(LogService logService)
        {
            _logService = logService;
        }

        [HttpGet("GetLogNames")]
        public List<string> GetLogNames()
        {
            return _logService.GetLogNames();
        }

        [HttpPost("GetLog")]
        public FileResult GetLog([FromForm]string filename)
        {
            LogFile logFile = _logService.GetLog(filename);
            return File(logFile.Memory, "text/plain", logFile.Filename);
        }
    } 
}

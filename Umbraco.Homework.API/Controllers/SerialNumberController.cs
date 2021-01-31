using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Homework.API.Models;
using Umbraco.Homework.API.Services;
using System.Linq;

namespace Umbraco.Homework.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SerialNumberController : ControllerBase
    {
        private readonly ISerialNumberService _serialNumberService;

        public SerialNumberController(ISerialNumberService serialNumberService)
        {
            _serialNumberService = serialNumberService;
        }

        [HttpGet("GetAllCurrentValidSerialNumbers")]
        public IActionResult GetAllCurrentValidSerialNumbers() => new JsonResult(this._serialNumberService.GetAllCurrentValidSerialNumbers().Select(e => e.Code));

        [HttpGet("ValidateSerialNumber")]
        public IActionResult ValidateSerialNumber(String serialNumber) => new JsonResult(this._serialNumberService.ValidateSerialNumber(serialNumber));

        [HttpGet("GenerateSerialNumberRange")]
        public async Task<ActionResult> GenerateSerialNumberRange(Int32 howMany)
        {
            if(howMany < 1)
            {
                return new JsonResult("The number of serial numbers to create must be more than 0");
            }

            return new JsonResult(await this._serialNumberService.GenerateSerialNumberRange(howMany));
        }
    }
}

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

        [HttpGet("GetAllValid")]
        public IActionResult GetAllValid() => new JsonResult(this._serialNumberService.GetAllValidSerialNumbers().Select(e => e.Code));

        [HttpGet("validate")]
        public IActionResult Validate(String serialNumber) => new JsonResult(this._serialNumberService.ValidateSerialNumber(serialNumber));

        [HttpGet("create")]
        public async Task<ActionResult> Create(Int32 howMany)
        {
            if(howMany < 1)
            {
                return new JsonResult("The number of serial numbers to create must be more than 0");
            }

            return new JsonResult(await this._serialNumberService.CreateRange(howMany));
        }
    }
}

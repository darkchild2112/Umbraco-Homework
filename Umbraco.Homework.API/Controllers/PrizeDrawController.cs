using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Umbraco.Homework.API.Exceptions;
using Umbraco.Homework.API.Models;
using Umbraco.Homework.API.Services;

namespace Umbraco.Homework.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrizeDrawController : ControllerBase
    {
        private readonly IPrizeDrawService _prizeDrawService;

        public PrizeDrawController(IPrizeDrawService prizeDrawService)
        {
            _prizeDrawService = prizeDrawService;
        }

        [HttpGet("getAll")]
        public IActionResult GetAll()
        { 
            return Ok(_prizeDrawService.GetAll());
        }

        // TODO: Create a method to validate the serial number

        // TODO: create a method to generated the serial numbers which have an expiry date

        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] PrizeDrawEntry entry)
        {
            // return new JsonResult("Serial Number was not valid")

            try
            {
                await _prizeDrawService.Create(entry);

                return Ok();
            }
            catch(InvalidSerialNumberException)
            {
                return BadRequest(new JsonResult("Invalid Serial Number"));
            }
        }
    }
}

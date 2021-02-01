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

        [HttpGet("GetAllEntries")]
        public IActionResult GetAllEntries()
        { 
            return Ok(_prizeDrawService.GetAllEntries());
        }

        // TODO: Create a method to validate the serial number

        // TODO: create a method to generated the serial numbers which have an expiry date

        [HttpPost("SubmitEntry")]
        public async Task<IActionResult> SubmitEntry([FromBody] PrizeDrawEntry entry)
        {
            // TEMP CODE - So that I can see the spinner when styling
            //Thread.Sleep(10000);

            try
            {
                await _prizeDrawService.SubmitEntry(entry);

                return Ok();
            }
            catch(InvalidUserInputException ex)
            {
                return BadRequest(new JsonResult(new { Message = "Invalid User Input", Errors = ex.Errors }));
            }
        }
    }
}

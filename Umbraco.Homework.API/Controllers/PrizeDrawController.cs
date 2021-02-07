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

        // TODO: Create an API method to validate the serial number so that it can be validated up front by the API before submitting

        [HttpPost("SubmitEntry")]
        public async Task<IActionResult> SubmitEntry([FromBody] PrizeDrawEntrySubmission entry)
        {
            try
            {
                await _prizeDrawService.SubmitEntryAsync(entry);

                return Ok();
            }
            catch(InvalidUserInputException ex)
            {
                return BadRequest(new JsonResult(new { Message = "Looks like there's a problem!", Errors = ex.Errors }));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Umbraco.Homework.Model;

namespace Umbraco.Homework.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrizeDrawController : ControllerBase
    {
        private readonly ILogger<PrizeDrawController> _logger;

        public PrizeDrawController(ILogger<PrizeDrawController> logger)
        {
            _logger = logger;
        }

        [HttpGet("getAll")]
        public ActionResult<IEnumerable<String>> GetAll()
        {
            return new List<String> { "SubmissionOne", "SubmissionTwo" };
        }

        [HttpPost("post")]
        public IActionResult Post(PrizeDrawEntry entry)
        {


            return Ok();
        }
    }
}

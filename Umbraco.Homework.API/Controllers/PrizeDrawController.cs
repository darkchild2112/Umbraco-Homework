using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        [HttpGet]
        public async Task<IEnumerable<String>> GetAll()
        {
            return new List<String> { "SubmissionOne", "SubmissionTwo" };
        }
    }
}

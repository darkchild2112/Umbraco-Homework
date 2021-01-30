using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Umbraco.Homework.Model;
using Umbraco.Homework.Services;

namespace Umbraco.Homework.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;

        private readonly IConfigService _configService;

        public ConfigController(ILogger<ConfigController> logger, IConfigService configService)
        {
            _logger = logger;
            _configService = configService;
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            Config config = _configService.GetConfig();

            return new JsonResult(config);
        }
    }
}

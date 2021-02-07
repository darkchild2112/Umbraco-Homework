using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Umbraco.Homework.API.Helpers;
using Umbraco.Homework.API.Models;

namespace Umbraco.Homework.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new Config
            {
                MaxSubmissions = this._configuration.GetValue<Int32>("MaxEntries"),
                Validation = ValidationHelper.GetValidation(this._configuration)
            });
        }
    }
}

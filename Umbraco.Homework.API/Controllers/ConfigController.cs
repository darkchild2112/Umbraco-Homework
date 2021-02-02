using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Umbraco.Homework.API.Models;
using Umbraco.Homework.API.Services;

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
                MaxSubmissions = this._configuration.GetValue<Int32>("MaxAllowedPrizeDrawEntries"),

                // TODO: Construct this more efficiently
                Validation = new PrizeDrawValidation
                {
                    FirstNameRules = this._configuration.GetSection("Validation:FirstNameRules").Get<IEnumerable<ValidationRule>>(),
                    LastNameRules = this._configuration.GetSection("Validation:LastNameRules").Get<IEnumerable<ValidationRule>>(),
                    EmailRules = this._configuration.GetSection("Validation:EmailRules").Get<IEnumerable<ValidationRule>>(),
                    SerialNumberRules = this._configuration.GetSection("Validation:SerialNumberRules").Get<IEnumerable<ValidationRule>>()
                }
            });
        }
    }
}

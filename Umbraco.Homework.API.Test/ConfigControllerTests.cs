using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Umbraco.Homework.API.Controllers;
using Umbraco.Homework.API.Models;
using Xunit;

namespace Umbraco.Homework.API.Test
{
    public class ConfigControllerTests : BaseTests
    {
        [Fact]
        public void TestInstantiation()
        {
            IConfiguration configuration = base.GetConfiguration();

            ConfigController controller = new ConfigController(configuration);

            Assert.NotNull(controller);
        }

        [Fact]
        public void TestGetConfigResult()
        {
            IConfiguration configuration = base.GetConfiguration();

            ConfigController controller = new ConfigController(configuration);

            IActionResult test = controller.Get();

            var okResult = test as OkObjectResult;

            // assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void TestMaxAllowedPrizeDrawEntries()
        {
            IConfiguration configuration = base.GetConfiguration(new Dictionary<string, string>
            {
                { "MaxEntries", "2" }
            });

            ConfigController controller = new ConfigController(configuration);

            IActionResult test = controller.Get();

            OkObjectResult okResult = test as OkObjectResult;

            Config config = okResult.Value as Config;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(2, config.MaxSubmissions);
        }

        // Can't get this working :(
        /*
        [Fact]
        public void TestValidation()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "MaxAllowedPrizeDrawEntries", "2" },
                { "Validation", "\"FirstNameRules\": [ { \"Regex\": \"\\S\", \"ErrorMessage\": \"First name is mandatory\" } ]" }
                                
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            ConfigController controller = new ConfigController(configuration);

            IActionResult test = controller.Get();

            OkObjectResult okResult = test as OkObjectResult;

            Config config = okResult.Value as Config;

            // assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(config.Validation.FirstNameRules);
        }
        */
    }
}

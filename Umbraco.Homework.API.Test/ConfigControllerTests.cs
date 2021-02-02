using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Umbraco.Homework.API.Controllers;
using Umbraco.Homework.API.Models;
using Xunit;

// [Theory] you pass in data
// [Fact] no data passed in to the method

// TEST DOUBLES
// Fakes - working implementations of functionality not fit for prd
// Dummies - can be created and passed arround but never used by the method we are testing
// Stubs - create stubs and pass them as dependencies to provide answers to calls on functionality
// Mocks - ??

// MockBehaviour.Strict - will throw an exception if a method is called on the mock object that hasn't been setup
// MockBehaviour.Loose (Default) - won't throw
// Example: Mock<IRepository<Bitmap>> mockImgRepo = new Mock<IRepository<Bitmap>>(MockBehavior.Strict);

namespace Umbraco.Homework.API.Test
{
    public class ConfigControllerTests
    {
        [Fact]
        public void TestInstantiation()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>())
                .Build();

            ConfigController controller = new ConfigController(configuration);

            Assert.NotNull(controller);
        }

        [Fact]
        public void TestGetConfigResult()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>())
                .Build();

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
            var inMemorySettings = new Dictionary<string, string>
            {
                { "MaxAllowedPrizeDrawEntries", "2" }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

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

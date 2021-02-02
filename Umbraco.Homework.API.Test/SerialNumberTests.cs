using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Umbraco.Homework.API.Controllers;
using Umbraco.Homework.API.Data;
using Umbraco.Homework.API.Models;
using Umbraco.Homework.API.Services;
using Xunit;
using System.Linq;

namespace Umbraco.Homework.API.Test
{

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

    public class SerialNumberTests
    {
        [Fact]
        public void TestInstantiation()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>())
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PrizeDrawDbContext>();

            optionsBuilder.UseInMemoryDatabase("PrizeDrawDatabseName");

            var context = new PrizeDrawDbContext(optionsBuilder.Options);

            ISerialNumberService serialNumberService = new SerialNumberService(context, configuration);

            SerialNumberController controller = new SerialNumberController(serialNumberService);

            Assert.NotNull(controller);
        }

        /*
            IActionResult test = controller.Get();

            OkObjectResult okResult = test as OkObjectResult;

            Config config = okResult.Value as Config;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(2, config.MaxSubmissions);
        */

        [Fact]
        public void TestGenerateSerialNumberRange()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>())
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PrizeDrawDbContext>();

            optionsBuilder.UseInMemoryDatabase("PrizeDrawDatabseName");

            var context = new PrizeDrawDbContext(optionsBuilder.Options);

            ISerialNumberService serialNumberService = new SerialNumberService(context, configuration);

            SerialNumberController controller = new SerialNumberController(serialNumberService);

            IActionResult test = controller.GenerateSerialNumberRange().Result;

            OkObjectResult okResult = test as OkObjectResult;

            IEnumerable<SerialNumber> serialNumbers = okResult.Value as IEnumerable<SerialNumber>;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(100, serialNumbers.Count());

            Assert.NotNull(controller);
        }
    }
}

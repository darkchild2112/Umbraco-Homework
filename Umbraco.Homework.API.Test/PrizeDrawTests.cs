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
using System.Threading;
using System.Threading.Tasks;

namespace Umbraco.Homework.API.Test
{
    public class PrizeDrawTests : BaseTests
    {
        [Fact]
        public void TestInstantiation()
        {
            IConfiguration configuration = GetConfiguration(null);

            var optionsBuilder = new DbContextOptionsBuilder<PrizeDrawDbContext>();

            optionsBuilder.UseInMemoryDatabase("PrizeDrawDatabseName");

            var context = new PrizeDrawDbContext(optionsBuilder.Options);

            ISerialNumberService serialNumberService = new SerialNumberService(context, configuration);

            IPrizeDrawService prizedrawService = new PrizeDrawService(context, serialNumberService, configuration);

            PrizeDrawController controller = new PrizeDrawController(prizedrawService);

            Assert.NotNull(controller);
        }

        [Fact]
        public async void TestSubmittingPrizeEntry()
        {
            IConfiguration configuration = GetConfiguration(null);

            var optionsBuilder = new DbContextOptionsBuilder<PrizeDrawDbContext>();

            optionsBuilder.UseInMemoryDatabase("PrizeDrawDatabseName");

            var context = new PrizeDrawDbContext(optionsBuilder.Options);

            ISerialNumberService serialNumberService = new SerialNumberService(context, configuration);

            IPrizeDrawService prizedrawService = new PrizeDrawService(context, serialNumberService, configuration);

            PrizeDrawController controller = new PrizeDrawController(prizedrawService);

            Assert.NotNull(controller);

            IEnumerable<SerialNumber> serialNumbers = await serialNumberService.GenerateSerialNumberRange(1);

            base.GetControllerResultAsync(controller.SubmitEntry, new PrizeDrawEntry
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "aaa@sss.com",
                SerialNumber = serialNumbers.FirstOrDefault()?.Code,
            });
        }
    }
}

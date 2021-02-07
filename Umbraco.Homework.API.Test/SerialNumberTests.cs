﻿using System;
using System.Collections.Generic;
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
    public class SerialNumberTests : BaseTests
    {
        [Fact]
        public void TestInstantiation()
        {
            IConfiguration configuration = GetConfiguration(null);

            var optionsBuilder = new DbContextOptionsBuilder<PrizeDrawDbContext>();

            optionsBuilder.UseInMemoryDatabase("PrizeDrawDatabseName");

            var context = new PrizeDrawDbContext(optionsBuilder.Options);

            ISerialNumberService serialNumberService = new SerialNumberService(context, configuration);

            SerialNumberController controller = new SerialNumberController(serialNumberService);

            Assert.NotNull(controller);
        }

        [Fact]
        public void TestGenerateSerialNumberRange()
        {
            IConfiguration config = GetConfiguration(null);

            var optionsBuilder = new DbContextOptionsBuilder<PrizeDrawDbContext>();

            optionsBuilder.UseInMemoryDatabase("PrizeDrawDatabseName");

            var context = new PrizeDrawDbContext(optionsBuilder.Options);

            ISerialNumberService serialNumberService = new SerialNumberService(context, config);

            SerialNumberController controller = new SerialNumberController(serialNumberService);

            IEnumerable<SerialNumber> serialNumbers = GetControllerResultAsync<IEnumerable<SerialNumber>, Int32>(controller.GenerateSerialNumberRange, 100).Result;

            Assert.Equal(100, serialNumbers.Count());

            Assert.NotNull(controller);
        }

        [Fact]
        public void TestSerialNumberExpiryPositive()
        {
            Int32 expiry = 600;

            IConfiguration config = GetConfiguration(
                new Dictionary<string, string>{
                    { "SerialNumberExpiryMilliseconds", expiry.ToString() },
                    { "MaxSerialNumberUses", "2" }
                });

            var optionsBuilder = new DbContextOptionsBuilder<PrizeDrawDbContext>();

            optionsBuilder.UseInMemoryDatabase("PrizeDrawDatabseName");

            var context = new PrizeDrawDbContext(optionsBuilder.Options);

            ISerialNumberService serialNumberService = new SerialNumberService(context, config);

            SerialNumberController controller = new SerialNumberController(serialNumberService);

            IEnumerable<SerialNumber> generatedSerialNumbers = GetControllerResultAsync<IEnumerable<SerialNumber>,Int32>(controller.GenerateSerialNumberRange, 10).Result;

            Assert.NotNull(generatedSerialNumbers);

            IEnumerable<String> validSerialNumbers = GetControllerResult<IEnumerable<String>>(controller.GetAllCurrentValidSerialNumbers);

            Assert.NotNull(validSerialNumbers);
            Assert.NotEmpty(validSerialNumbers);
        }

        /*
        [Fact]
        public void TestSerialNumberExpiryNegative()
        {
            Int32 expiry = 6;

            IConfiguration config = GetConfiguration(
                new Dictionary<string, string> {
                    { "SerialNumberExpiryMilliseconds", "0" },
                    { "MaxAllowedPrizeDrawEntries", "2" }
                });

            var optionsBuilder = new DbContextOptionsBuilder<PrizeDrawDbContext>();

            optionsBuilder.UseInMemoryDatabase("PrizeDrawDatabseName");

            var context = new PrizeDrawDbContext(optionsBuilder.Options);

            ISerialNumberService serialNumberService = new SerialNumberService(context, config);

            SerialNumberController controller = new SerialNumberController(serialNumberService);

            IEnumerable<SerialNumber> generatedSerialNumbers = GetControllerResultAsync<IEnumerable<SerialNumber>, Int32>(
                controller.GenerateSerialNumberRange, 10).Result;

            Assert.NotNull(generatedSerialNumbers);

            IEnumerable<String> validSerialNumbers = GetControllerResult<IEnumerable<String>>(controller.GetAllCurrentValidSerialNumbers);

            Assert.NotNull(validSerialNumbers);
            Assert.Empty(validSerialNumbers);
        }
        */
    }
}

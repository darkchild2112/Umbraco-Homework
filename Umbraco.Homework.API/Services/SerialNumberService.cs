﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Homework.API.Data;
using Umbraco.Homework.API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Umbraco.Homework.API.Services
{
    public class SerialNumberService : ServiceBase, ISerialNumberService
    {
        private Int32 MaxUses => this._configuration.GetValue<Int32>("MaxSerialNumberUses", 1);

        private Int32 ExpiryMilliseconds => this._configuration.GetValue<Int32>("SerialNumberExpiryMilliseconds", 100000); 

        private readonly IConfiguration _configuration;

        public SerialNumberService(PrizeDrawDbContext dataAccess, IConfiguration configuration):base(dataAccess)
        {
            this._configuration = configuration;
        }


        public async Task<IEnumerable<SerialNumber>> GenerateSerialNumberRangeAsync(Int32 nToCreate)
        {

            // Slower than the sync method for some reason - possibly due to the overhead of creating each thread
            // Later found that it could also be swagger not able to handle large JSON objects - https://github.com/swagger-api/swagger-ui/issues/3832
            //IEnumerable<SerialNumber> sNumbers = await GenerateSerialNumberRangeParallelAsync(nToCreate);

            IEnumerable<SerialNumber> sNumbers = GenerateSerialNumberRange(nToCreate);

            base._dataAccess.AddRange(sNumbers);

            await base.Save();

            return sNumbers;
        }

        public void IncrementSerialNumberUses(String serialNumber)
        {
            SerialNumber sn = this._dataAccess.SerialNumbers
                .Where(e => e.Code.ToLower() == serialNumber.ToLower())
                .FirstOrDefault();

            IncrementSerialNumberUses(sn);
        }

        public void IncrementSerialNumberUses(SerialNumber sn)
        {
            sn.Uses++;
        }

        public Boolean ValidateSerialNumber(String serialNumber)
        {
            Boolean valid = false;

            // String.Equals doesn't work here :(
            SerialNumber sn = this._dataAccess.SerialNumbers
                .AsNoTracking()
                .Where(e => e.Code.ToLower() == serialNumber.ToLower())
                .FirstOrDefault();

            if(sn != null)
            {
                valid = sn.ValidUnitl > DateTime.Now && sn.Uses < this.MaxUses;
            }

            return valid;
        }

        public IEnumerable<SerialNumber> GetAllCurrentValidSerialNumbers()
            => this._dataAccess.SerialNumbers
                .AsNoTracking()
                .Where(sn => sn.ValidUnitl > DateTime.Now
                && sn.Uses < this.MaxUses);

        public SerialNumber GetSerialNumber(String code)
            => this._dataAccess.SerialNumbers
            .Where(e => e.Code == code)
            .FirstOrDefault();

        // -----------------------

        private async Task<IEnumerable<SerialNumber>> GenerateSerialNumberRangeParallelAsync(Int32 nToCreate)
        {
            List<Task<SerialNumber>> tasks = new List<Task<SerialNumber>>();

            DateTime validUntil = DateTime.Now.AddMilliseconds(this.ExpiryMilliseconds);

            for (Int32 i = 0; i < nToCreate; i++)
            {
                tasks.Add(Task.Run(() => GenerateSerialNumber(validUntil)));
            };

            await Task.WhenAll(tasks);

            return tasks.Select(e => e.Result);
        }

        private IEnumerable<SerialNumber> GenerateSerialNumberRange(Int32 nToCreate)
        {
            List<SerialNumber> serialNumbers = new List<SerialNumber>();

            DateTime validUntil = DateTime.Now.AddMilliseconds(this.ExpiryMilliseconds);

            for (Int32 i = 0; i < nToCreate; i++)
            {
                serialNumbers.Add(GenerateSerialNumber(validUntil));
            };

            return serialNumbers;
        }

        private SerialNumber GenerateSerialNumber(DateTime validUntill)
            => new SerialNumber
            {
                Code = Guid.NewGuid().ToString(),
                ValidUnitl = validUntill
            };
    }
}

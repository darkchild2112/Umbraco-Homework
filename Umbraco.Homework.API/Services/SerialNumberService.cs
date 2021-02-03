using System;
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
        private Int32 MaxUses => this._configuration.GetValue<Int32>("MaxAllowedPrizeDrawEntries", 1);

        private Int32 ExpiryMilliseconds => this._configuration.GetValue<Int32>("SerialNumberExpiryMilliseconds", 100000); 

        private readonly IConfiguration _configuration;

        public SerialNumberService(PrizeDrawDbContext dataAccess, IConfiguration configuration):base(dataAccess)
        {
            this._configuration = configuration;
        }

        public async Task<IEnumerable<SerialNumber>> GenerateSerialNumberRange(Int32 nToCreate)
        {
            List<SerialNumber> sNumbers = new List<SerialNumber>();

            for(Int32 i =0; i < nToCreate; i++)
            {
                SerialNumber sNumber = new SerialNumber
                {
                    Code = Guid.NewGuid().ToString(),
                    ValidUnitl = DateTime.Now.AddMilliseconds(this.ExpiryMilliseconds)
                };

                sNumbers.Add(sNumber);
            };

            base._dataAccess.AddRange(sNumbers);

            await base.Save();

            return sNumbers;
        }

        public void IncrementSerialNumberUses(String serialNumber)
        {
            SerialNumber sn = this._dataAccess.SerialNumbers
                .Where(e => e.Code.ToLower() == serialNumber.ToLower())
                .FirstOrDefault();

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
    }
}

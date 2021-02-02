using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Homework.API.Data;
using Umbraco.Homework.API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Umbraco.Homework.API.Services
{
    public class SerialNumberService : ServiceBase, ISerialNumberService
    {
        public SerialNumberService(PrizeDrawDbContext dataAccess):base(dataAccess)
        {
        }

        public async Task<IEnumerable<SerialNumber>> GenerateSerialNumberRange(Int32 nToCreate)
        {
            List<SerialNumber> sNumbers = new List<SerialNumber>();

            Parallel.For(0, nToCreate, i =>
            {
                SerialNumber sNumber = new SerialNumber
                {
                    Code = Guid.NewGuid().ToString(),
                    ValidUnitl = DateTime.Now.AddMinutes(10)
                };

                sNumbers.Add(sNumber);
            });

            base._dataAccess.AddRange(sNumbers);

            await base.Save();

            return sNumbers;
        }

        public Boolean ValidateSerialNumber(String serialNumber)
        {
            Boolean valid = false;

            // String.Equals doesn't work here :(
            SerialNumber sn = this._dataAccess.SerialNumbers
                .AsNoTracking()
                .Where(e => e.Code.ToLower() == serialNumber
                .ToLower())
                .FirstOrDefault();

            if(sn != null)
            {
                valid = sn.ValidUnitl > DateTime.Now;
            }

            return valid;
        }

        public IEnumerable<SerialNumber> GetAllCurrentValidSerialNumbers()
            => this._dataAccess.SerialNumbers
            .AsNoTracking()
            .Where(e => e.ValidUnitl > DateTime.Now);
    }
}

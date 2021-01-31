using System;
using System.Collections.Generic;
using Umbraco.Homework.API.Data;
using Umbraco.Homework.API.Models;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Homework.API.Exceptions;

namespace Umbraco.Homework.API.Services
{
    public class PrizeDrawService : ServiceBase, IPrizeDrawService
    {
        private readonly ISerialNumberService _serialNumberService;

        public PrizeDrawService(PrizeDrawDbContext dataAccess, ISerialNumberService serialNumberService):base(dataAccess)
        {
            this._serialNumberService = serialNumberService;
        }

        public IEnumerable<PrizeDrawEntry> GetAll() => base._dataAccess.PrizeDrawEntries;

        public async Task<PrizeDrawEntry> Create(PrizeDrawEntry entry)
        {

            Boolean isValidSerialNumber = this._serialNumberService.ValidateSerialNumber(entry.SerialNumber);

            if(!isValidSerialNumber)
            {
                // Throw a custom Exception
                throw new InvalidSerialNumberException();
            }


            base._dataAccess.Add<PrizeDrawEntry>(entry);

            await base.Save();

            return entry;
        }

        
    }
}

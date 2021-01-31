using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Homework.API.Models;

namespace Umbraco.Homework.API.Services
{
    public interface IPrizeDrawService
    {
        IEnumerable<PrizeDrawEntry> GetAll();

        Task<PrizeDrawEntry> Create(PrizeDrawEntry entry);
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Umbraco.Homework.API.Data;

namespace Umbraco.Homework.API.Services
{
    public abstract class ServiceBase
    {
        protected readonly PrizeDrawDbContext _dataAccess;

        public ServiceBase(PrizeDrawDbContext dataAccess)
        {
            this._dataAccess = dataAccess;
        }

        protected async Task Save()
        {
            try
            {
                await this._dataAccess.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Todo: log the error

                throw;
                // or return false???
            }
        }
    }
}

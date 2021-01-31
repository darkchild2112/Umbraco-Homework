using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Homework.API.Models;

namespace Umbraco.Homework.API.Services
{
    public interface ISerialNumberService
    {
        Task<IEnumerable<SerialNumber>> GenerateSerialNumberRange(Int32 nToCreate);

        Boolean ValidateSerialNumber(String serialNumber);

        IEnumerable<SerialNumber> GetAllCurrentValidSerialNumbers();
    }
}

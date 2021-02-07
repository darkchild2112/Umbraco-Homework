using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Homework.API.Models;

namespace Umbraco.Homework.API.Services
{
    public interface ISerialNumberService
    {
        Task<IEnumerable<SerialNumber>> GenerateSerialNumberRangeAsync(Int32 nToCreate);

        Boolean ValidateSerialNumber(String serialNumber);

        IEnumerable<SerialNumber> GetAllCurrentValidSerialNumbers();

        void IncrementSerialNumberUses(String serialNumber);

        void IncrementSerialNumberUses(SerialNumber sn);

        SerialNumber GetSerialNumber(String code);
    }
}

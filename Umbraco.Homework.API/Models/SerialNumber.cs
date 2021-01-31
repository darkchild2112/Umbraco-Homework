using System;
namespace Umbraco.Homework.API.Models
{
    public class SerialNumber
    {
        public Int32 Id { get; set; }
        public String Code { get; set; }
        public DateTime ValidUnitl { get; set; }

        public SerialNumber()
        {
        }
    }
}

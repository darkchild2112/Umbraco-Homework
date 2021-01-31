using System;

namespace Umbraco.Homework.API.Models
{
    public class PrizeDrawEntry
    {
        public Int32 Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String SerialNumber { get; set; }

        public PrizeDrawEntry()
        {
        }
    }
}

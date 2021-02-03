using System;
namespace Umbraco.Homework.API.Models
{
    public class PrizeDrawEntrySubmission
    {
        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Email { get; set; }

        public String SerialNumber { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}

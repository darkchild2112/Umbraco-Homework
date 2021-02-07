using System.Collections.Generic;

namespace Umbraco.Homework.API.Models
{
    public class PrizeDrawValidation
    {
        public IEnumerable<ValidationRule> FirstNameRules { get; set; }

        public IEnumerable<ValidationRule> LastNameRules { get; set; }

        public IEnumerable<ValidationRule> EmailRules { get; set; }

        public IEnumerable<ValidationRule> SerialNumberRules { get; set; }

        public IEnumerable<ValidationRule> DateOfBirthRules { get; set; }

        public PrizeDrawValidation()
        {
        }
    }
}

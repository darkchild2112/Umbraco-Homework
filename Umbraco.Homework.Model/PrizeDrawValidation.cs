using System;
using System.Collections.Generic;

namespace Umbraco.Homework.Model
{
    public class PrizeDrawValidation
    {
        public IEnumerable<ValidationRule> FirstNameRules { get; set; }

        public IEnumerable<ValidationRule> LastNameRules { get; set; }

        public IEnumerable<ValidationRule> EmailRules { get; set; }

        public IEnumerable<ValidationRule> SerialNumberRules { get; set; }

        public PrizeDrawValidation()
        {
        }
    }
}

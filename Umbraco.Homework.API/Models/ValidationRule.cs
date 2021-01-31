using System;

namespace Umbraco.Homework.API.Models
{
    public class ValidationRule
    {
        public String Regex { get; set; }

        public String ErrorMessage { get; set; }

        public ValidationRule()
        {
        }
    }
}

using System;
namespace Umbraco.Homework.Model
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

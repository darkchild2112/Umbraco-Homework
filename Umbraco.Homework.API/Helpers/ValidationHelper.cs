using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Umbraco.Homework.API.Models;

namespace Umbraco.Homework.API.Helpers
{
    public static class ValidationHelper
    {
        public static PrizeDrawValidation GetValidation(IConfiguration configuration)
        {
            return new PrizeDrawValidation
            {
                FirstNameRules = configuration.GetSection("Validation:FirstNameRules").Get<IEnumerable<ValidationRule>>(),
                LastNameRules = configuration.GetSection("Validation:LastNameRules").Get<IEnumerable<ValidationRule>>(),
                EmailRules = configuration.GetSection("Validation:EmailRules").Get<IEnumerable<ValidationRule>>(),
                SerialNumberRules = configuration.GetSection("Validation:SerialNumberRules").Get<IEnumerable<ValidationRule>>(),
                DateOfBirthRules = configuration.GetSection("Validation:DateOfBirthRules").Get<IEnumerable<ValidationRule>>()
            };
        }
    }
}

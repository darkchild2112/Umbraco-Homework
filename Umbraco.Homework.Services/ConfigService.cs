using System;
using System.Collections.Generic;
using Umbraco.Homework.Model;

namespace Umbraco.Homework.Services
{
    public class ConfigService : IConfigService
    {
        // Temp hard coded for now
        public Config GetConfig()
        {
            return new Config { MaxSubmissions = 2, Validation = this.GetValidationRules() };
        }

        // TODO: The content should come from a content service
        private PrizeDrawValidation GetValidationRules()
        {
            return new PrizeDrawValidation
            {
                FirstNameRules = new List<ValidationRule> {
                    new ValidationRule { Regex = @"\S", ErrorMessage= "first name is mandatory" },
                },
                LastNameRules = new List<ValidationRule> {
                    new ValidationRule { Regex = @"\S", ErrorMessage= "last name is mandatory" },
                },
                EmailRules = new List<ValidationRule> {
                    new ValidationRule { Regex = @"\S", ErrorMessage= "email is mandatory" },
                    new ValidationRule { Regex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:.[a-zA-Z0-9-]+)*$", ErrorMessage= "email needs to be in the format xxx@sss.xxx" }
                },
                SerialNumberRules = new List<ValidationRule> {
                    new ValidationRule { Regex = @"\S", ErrorMessage= "serial number is mandatory" },
                }
            };
        }
    }
}

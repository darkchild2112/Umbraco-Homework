using System;
using System.Collections.Generic;
using Umbraco.Homework.API.Data;
using Umbraco.Homework.API.Models;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Homework.API.Exceptions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Umbraco.Homework.API.Services
{
    public class PrizeDrawService : ServiceBase, IPrizeDrawService
    {
        private readonly ISerialNumberService _serialNumberService;
        private readonly IConfiguration _configuration;

        public PrizeDrawService(PrizeDrawDbContext dataAccess, ISerialNumberService serialNumberService, IConfiguration configuration):base(dataAccess)
        {
            this._serialNumberService = serialNumberService;
            this._configuration = configuration;
        }

        public IEnumerable<PrizeDrawEntry> GetAllEntries() => base._dataAccess.PrizeDrawEntries;

        public async Task<PrizeDrawEntry> SubmitEntry(PrizeDrawEntry entry)
        {
            _ = entry ?? throw new NullReferenceException($"{nameof(PrizeDrawEntry)} must not be null");

            // TODO: Construct this more efficiently
            PrizeDrawValidation validation = new PrizeDrawValidation
            {
                FirstNameRules = this._configuration.GetSection("Validation:FirstNameRules").Get<IEnumerable<ValidationRule>>(),
                LastNameRules = this._configuration.GetSection("Validation:LastNameRules").Get<IEnumerable<ValidationRule>>(),
                EmailRules = this._configuration.GetSection("Validation:EmailRules").Get<IEnumerable<ValidationRule>>(),
                SerialNumberRules = this._configuration.GetSection("Validation:SerialNumberRules").Get<IEnumerable<ValidationRule>>()
            };

            (Boolean isValid, IEnumerable<String> errors) validationResult = this.ValidateUserInput(validation, entry);

            if (!validationResult.isValid)
            {
                throw new InvalidUserInputException("Invalid user input from prize draw entry", validationResult.errors);
            }

            base._dataAccess.Add<PrizeDrawEntry>(entry);

            await base.Save();

            return entry;
        }

        private (Boolean, IEnumerable<String>) ValidateUserInput(PrizeDrawValidation validation, PrizeDrawEntry entry)
        {
            List<String> errors = new List<String>();

            (Boolean valid, IEnumerable<String> errors) firstNameValidationResults = this.ValidateField(validation.FirstNameRules, entry.FirstName);
            (Boolean valid, IEnumerable<String> errors) lastNameValidationResults = this.ValidateField(validation.LastNameRules, entry.LastName);
            (Boolean valid, IEnumerable<String> errors) emailValidationResults = this.ValidateField(validation.EmailRules, entry.Email);
            (Boolean valid, IEnumerable<String> errors) serailNumberValidationResults = this.ValidateField(validation.SerialNumberRules, entry.SerialNumber);

            Boolean inputValid = firstNameValidationResults.valid && lastNameValidationResults.valid && emailValidationResults.valid && serailNumberValidationResults.valid;

            Boolean? isValidSerialNumber = null;

            if(inputValid == false)
            {
                errors.AddRange(firstNameValidationResults.errors);
                errors.AddRange(lastNameValidationResults.errors);
                errors.AddRange(emailValidationResults.errors);
                errors.AddRange(serailNumberValidationResults.errors);
            }

            if(serailNumberValidationResults.valid == true)
            {
                isValidSerialNumber = this._serialNumberService.ValidateSerialNumber(entry.SerialNumber);
            }

            if (isValidSerialNumber.HasValue && isValidSerialNumber.Value == false)
            {
                errors.Add("Serial Number has expired");
            }

            return (inputValid && isValidSerialNumber.HasValue && isValidSerialNumber.Value, errors);
        }

        private (Boolean, IEnumerable<String>) ValidateField(IEnumerable<ValidationRule> rules, String value)
        {
            Boolean valid = true;
            List<String> errorMessages = new List<String>();

            foreach (ValidationRule rule in rules)
            {
                Regex regex = new Regex(rule.Regex);

                valid = regex.IsMatch(value.ToLower());

                if(valid == false)
                {
                    errorMessages.Add(rule.ErrorMessage);
                }
            }

            return (valid, errorMessages);
        }
    }
}

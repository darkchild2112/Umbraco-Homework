using System;
using System.Collections.Generic;
using Umbraco.Homework.API.Data;
using Umbraco.Homework.API.Models;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Homework.API.Exceptions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<PrizeDrawEntry> GetAllEntries()
            => base._dataAccess.PrizeDrawEntries
            .AsNoTracking();

        public async Task<PrizeDrawEntry> SubmitEntry(PrizeDrawEntrySubmission userInput)
        {
            _ = userInput ?? throw new NullReferenceException($"{nameof(PrizeDrawEntrySubmission)} must not be null");

            (Boolean isValid, IEnumerable<String> errors) validationResult = this.ValidateUserInput(userInput);

            if (!validationResult.isValid)
            {
                throw new InvalidUserInputException("Invalid user input from prize draw entry", validationResult.errors);
            }

            SerialNumber serialNumber = this._serialNumberService.GetSerialNumber(userInput.SerialNumber);

            this._serialNumberService.IncrementSerialNumberUses(serialNumber);

            PrizeDrawEntry entry = new PrizeDrawEntry
            {
                 FirstName = userInput.FirstName,
                 LastName = userInput.LastName,
                 Email = userInput.Email,
                 Submitted = DateTime.Now,
                 SerialNumber = serialNumber
            };

            base._dataAccess.Add<PrizeDrawEntry>(entry);

            await base.Save();

            return entry;
        }

        // ----------------------------------------------------

        // TODO: Async this!!
        private (Boolean, IEnumerable<String>) ValidateUserInput(PrizeDrawEntrySubmission entry)
        {
            List<String> errors = new List<String>();

            IConfigurationSection validationSection = this._configuration.GetSection("Validation");

            Boolean inputValid = true;

            if (validationSection.Value != null)
            {
                // TODO: Construct this more efficiently
                PrizeDrawValidation validation = new PrizeDrawValidation
                {
                    FirstNameRules = validationSection.GetSection("FirstNameRules").Get<IEnumerable<ValidationRule>>(),
                    LastNameRules = validationSection.GetSection("LastNameRules").Get<IEnumerable<ValidationRule>>(),
                    EmailRules = validationSection.GetSection("EmailRules").Get<IEnumerable<ValidationRule>>(),
                    SerialNumberRules = validationSection.GetSection("SerialNumberRules").Get<IEnumerable<ValidationRule>>()
                };

                (Boolean valid, IEnumerable<String> errors) firstNameValidationResults = this.ValidateField(validation.FirstNameRules, entry.FirstName);
                (Boolean valid, IEnumerable<String> errors) lastNameValidationResults = this.ValidateField(validation.LastNameRules, entry.LastName);
                (Boolean valid, IEnumerable<String> errors) emailValidationResults = this.ValidateField(validation.EmailRules, entry.Email);
                (Boolean valid, IEnumerable<String> errors) serailNumberValidationResults = this.ValidateField(validation.SerialNumberRules, entry.SerialNumber);

                inputValid = firstNameValidationResults.valid && lastNameValidationResults.valid && emailValidationResults.valid && serailNumberValidationResults.valid;

                if (inputValid == false)
                {
                    errors.AddRange(firstNameValidationResults.errors);
                    errors.AddRange(lastNameValidationResults.errors);
                    errors.AddRange(emailValidationResults.errors);
                    errors.AddRange(serailNumberValidationResults.errors);
                }
            }

            Boolean? isValidSerialNumber = null;

            if(inputValid)
            {
                isValidSerialNumber = this._serialNumberService.ValidateSerialNumber(entry.SerialNumber);
            }

            if (isValidSerialNumber.HasValue && isValidSerialNumber.Value == false)
            {
                errors.Add("Serial Number is no longer valid");
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

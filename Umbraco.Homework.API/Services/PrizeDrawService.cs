﻿using System;
using System.Collections.Generic;
using Umbraco.Homework.API.Data;
using Umbraco.Homework.API.Models;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Homework.API.Exceptions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Umbraco.Homework.API.Helpers;

namespace Umbraco.Homework.API.Services
{
    public class PrizeDrawService : ServiceBase, IPrizeDrawService
    {
        private readonly ISerialNumberService _serialNumberService;
        private readonly IConfiguration _configuration;

        private Int32 MaxEntries => this._configuration.GetValue<Int32>("MaxEntries");
        private Int32 MinAge => this._configuration.GetValue<Int32>("MinAge");

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
                PrizeDrawValidation validation = ValidationHelper.GetValidation(this._configuration);

                (Boolean valid, IEnumerable<String> errors) firstNameValidationResults = this.ValidateField(validation.FirstNameRules, entry.FirstName);
                (Boolean valid, IEnumerable<String> errors) lastNameValidationResults = this.ValidateField(validation.LastNameRules, entry.LastName);
                (Boolean valid, IEnumerable<String> errors) emailValidationResults = this.ValidateField(validation.EmailRules, entry.Email);
                (Boolean valid, IEnumerable<String> errors) serailNumberValidationResults = this.ValidateField(validation.SerialNumberRules, entry.SerialNumber);

                inputValid = firstNameValidationResults.valid
                    && lastNameValidationResults.valid
                    && emailValidationResults.valid
                    && serailNumberValidationResults.valid;

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

            Boolean oldEnough = ValidateAge(entry.DateOfBirth);

            if (!oldEnough)
            {
                errors.Add($"Not old enough sorry. min age is {this.MinAge}");
            }

            Boolean nEntriesValid = this.validateNumberOfEntries(entry.Email);

            if (!nEntriesValid)
            {
                errors.Add($"The maximum number of entries per email address is {this.MaxEntries}");
            }

            return (inputValid
                && isValidSerialNumber.HasValue
                && isValidSerialNumber.Value
                && oldEnough
                && nEntriesValid,
                errors);
        }

        private Boolean validateNumberOfEntries(String email)
        {
            var entries = from e
                          in this._dataAccess.PrizeDrawEntries
                          where e.Email == email
                          select e;

            Int32 entrie = entries.Count();

            return entries.Count() <= this.MaxEntries;
        }

        private Boolean ValidateAge(DateTime dateOfBirth)
        {
            Int32 age = DateTime.Now.Year - dateOfBirth.Year;

            if (dateOfBirth > DateTime.Now.AddYears(-age))
            {
                age--;
            }

            return age >= this.MinAge;
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

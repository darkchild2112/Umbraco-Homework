using System;

namespace Umbraco.Homework.API.Models
{
    public class Config
    {
        public Int32 MaxSubmissions { get; set; }

        public PrizeDrawValidation Validation { get; set; }
    }
}

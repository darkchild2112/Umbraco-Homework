using System;
using System.ComponentModel.DataAnnotations;

namespace Umbraco.Homework.API.Models
{
    public class SerialNumber
    {
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(200)]
        public String Code { get; set; }

        [Required]
        public DateTime ValidUnitl { get; set; }

        public SerialNumber()
        {
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Umbraco.Homework.API.Models
{
    public class PrizeDrawEntry
    {
        public Int32 Id { get; set; }

        [Required]
        // Otherwise all string are created as nvarchar(max)
        [MaxLength(200)] 
        public String FirstName { get; set; }

        [Required]
        [MaxLength(200)]
        public String LastName { get; set; }

        [Required]
        [MaxLength(500)]
        public String Email { get; set; }

        [Required]
        [MaxLength(200)]
        public String SerialNumber { get; set; }

        public PrizeDrawEntry()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AHR.Models
{
    public class AddUser
    {
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [StringLength(60, ErrorMessage = "Minimum 5 character is required", MinimumLength = 5)]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string UserType { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string MobileNumber { get; set; }

        [StringLength(60, ErrorMessage = "Minimum 5 character is required", MinimumLength = 5)]
        [DataType(DataType.EmailAddress)]
        public string EmailFirst { get; set; }

        [StringLength(60, ErrorMessage = "Minimum 5 character is required", MinimumLength = 5)]
        [DataType(DataType.EmailAddress)]
        public string EmailSecond { get; set; }

    }
}

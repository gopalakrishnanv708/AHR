using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AHR.Models
{

    public class ValidateDateRange : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            if (Convert.ToDateTime(value) <= DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Please select past date range");
            }
        }
    }

    public class Income
    {
        [Required]
        [DataType(DataType.Text)]
        public string DonorEmail { get; set; }

        
        [Required]
        [ValidateDateRange]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d{1,5}(\.\d{1,2})?$", ErrorMessage = "Enter value with maximum two decimal places and 5 digits. ex.99999.99")]
        public string PaymentAmount { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string PaymentMethod { get; set; }

        public List<string> PaymentMethodName;

        public List<IFormFile> Imagefile { get; set; }
    }

    
}

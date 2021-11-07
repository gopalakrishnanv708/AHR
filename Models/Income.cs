using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AHR.Models
{
    public class Income
    {
        [Required]
        [DataType(DataType.Text)]
        public string DonorEmail { get; set; }

        [Required]
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

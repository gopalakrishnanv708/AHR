using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AHR.Models
{
    public class Expense
    {
        [Required]
        [DataType(DataType.Text)]
        public string FamilyName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string ExpenseDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d{1,5}(\.\d{1,2})?$", ErrorMessage = "Enter value with maximum two decimal places and 5 digits. ex.99999.99")]
        public string ExpenseAmount { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string ExpensePurpose { get; set; }

        public List<IFormFile> Imagefile { get; set; }
    }
}

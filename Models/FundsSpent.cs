using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AHR.Models
{
    public class FundsSpent
    {
        [DataType(DataType.Text)]
        public int FamilyNo { get; set; }

        [DataType(DataType.Text)]
        public string FamilyName { get; set; }

        public string ExpenseDate { get; set; }

        [DataType(DataType.Currency)]
        public string ExpenseAmount { get; set; }

        [DataType(DataType.MultilineText)]
        public string ExpensePurpose { get; set; }

        public List<string> ProofDocument { get; set; }

    }

}

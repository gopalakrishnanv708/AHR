using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AHR.Models
{
    public class FundsReceived
    {
        
        public string PaymentDate { get; set; }
        
        public string PaymentAmount { get; set; }

        
        [DataType(DataType.Text)]
        public string PaymentMethod { get; set; }

    }
}

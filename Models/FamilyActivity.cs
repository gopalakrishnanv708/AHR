using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AHR.Models
{

    public class FamilyActivity
    {
        public string Date { get; set; }

        public string FamilyName { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        public List<string> Images { get; set; }

        public List<string> Video { get; set; }
    }
}

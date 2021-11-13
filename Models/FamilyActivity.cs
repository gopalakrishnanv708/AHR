using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AHR.Models
{

    public class FamilyActivityList
    {
        public List<FamilyActivity> FamilyActivities { get; set; }
    }

    public class FamilyActivity
    {
        [DataType(DataType.Text)]
        public string Description { get; set; }

        public List<string> Images { get; set; }

        public List<string> Video { get; set; }
    }
}

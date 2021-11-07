using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AHR.Models
{
    public class AhrActivity
    {
        [Required]
        [DataType(DataType.Text)]
        public string FamilyName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string ActivityDate { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string ActivityDesc { get; set; }

        public List<IFormFile> Imagefile { get; set; }

        public List<IFormFile> Videofile { get; set; }
    }
}

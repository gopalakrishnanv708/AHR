using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AHR.Models
{

    public class FamilyDetailsList
    {
        public List<FamilyDetails> FamilyDetails { get; set; }
    }

    public class FamilyDetails
    {
        [DataType(DataType.Text)]
        public int FamilyNo { get; set; }

        [DataType(DataType.Text)]
        public string FamilyName { get; set; }

        [DataType(DataType.Text)]
        public string FamilyMemberName { get; set; }

        [DataType(DataType.Text)]
        public string IsHead { get; set; }

        [DataType(DataType.Date)]
        public string DOB { get; set; }

        [DataType(DataType.Text)]
        public string PassportNumber { get; set; }

        [DataType(DataType.Date)]
        public string VisaExpiryDate { get; set; }

        [DataType(DataType.Text)]
        public string Nationality { get; set; }

        [DataType(DataType.Text)]
        public string RelationWithHead { get; set; }

        [DataType(DataType.Text)]
        public string PicOfMember { get; set; }
    }
}

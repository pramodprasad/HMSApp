using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Entity.MetaData
{
    class Metadata
    {
    }

    public class EmployeeMetaData
    {
        [Key]
        [Required]
        [ScaffoldColumn(false)]
        public long ID { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public Nullable<decimal> MobileNo { get; set; }
        [Display(Name = "Other Contact No")]
        public string OtherContactNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public long City { get; set; }
        [Display(Name = "Asdhar No")]
        public string AadharNo { get; set; }
        [Display(Name = "Voter ID No")]
        public string VoterIDNo { get; set; }
        [Display(Name = "Driving Licence No")]
        public string DriverLicenceNo { get; set; }
        public string CreatedDate { get; set; }
        [Required]
        [Display(Name = "Branch")]
        public long BranchID { get; set; }
        [Required]
        [Display(Name = "Employee Type")]
        public long EmployeeTypeID { get; set; }
        [Required]
        [Display(Name = "Title")]
        public Nullable<int> TitleID { get; set; }
    }

    public class UserTypeMetaData
    {

    }
}

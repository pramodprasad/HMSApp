using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Entity
{
    class MetaData
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
        [Display(Name = "Aadhar No")]
        public string AadharNo { get; set; }
        [Display(Name = "Voter ID No")]
        public string VoterIDNo { get; set; }
        [Display(Name = "Driving Licence No")]
        public string DriverLicenceNo { get; set; }
        public string CreatedDate { get; set; }
    }

    public class PatientMetaData
    {
        [Required(ErrorMessage="Patient name is required.")]
        [Display(Name = "Name")]
        public string FullName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required(ErrorMessage = "Father/Husband name is required.")]
        [Display(Name = "Father/Husband")]
        public string FatherOrHusbandName { get; set; }
        [Required(ErrorMessage = "Contact no is required.")]
        [Display(Name = "Contact No")]
        public long ContactNo { get; set; }
        [Display(Name = "Alt. Contact No")]
        public long OtherContactNo { get; set; }
        [Required(ErrorMessage = "Reg. Date is required.")]
        [Display(Name = "Reg. Date")]
        public System.DateTime DateOfRegistration { get; set; }
        [Required(ErrorMessage = "Marital status is required.")]
        [Display(Name = "Marital Status")]
        public string MarritalStatus { get; set; }
        [Required(ErrorMessage="Address is required.")]
        public string Address { get; set; }
        [Required]
        public int Gender { get; set; }
    }

    //public class PatientStatusMetaData
    //{
    //    [Display(Name = "Refferal")]
    //    public bool IsReferal { get; set; }
    //    [Display(Name = "Next Call")]
    //    public bool IsNextCall { get; set; }
    //    [Required]
    //    [Display(Name = "Visit Date")]
    //    [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
    //    public System.DateTime VisitDate { get; set; }
    //    [Display(Name = "Next Call Date")]
    //    [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
    //    public System.DateTime NextCallDate { get; set; }
    //    [Required]
    //    [Display(Name = "Payment Status")]
    //    public int PaymentStatusID { get; set; }
    //    [Display(Name = "Visit Time")]
    //    public int VisitedTime { get; set; }
    //    [Display(Name = "Discharge Date")]
    //    [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
    //    public System.DateTime DischargeDate { get; set; }
    //}

    public class DoctorMetaData
    {
        [Display(Name = "Other Details")]
        public string OtherDetails { get; set; }
        [Display(Name = "Reg No.")]
        public string RegistrationNo { get; set; }
        [Display(Name = "Created On")]
        public System.DateTime CreatedOn { get; set; }
        [Display(Name = "Updated On")]
        public System.DateTime UpdatedOn { get; set; }
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }
        [Display(Name = "Updated By")]
        public int UpdatedBy { get; set; }
        public bool Daywise { get; set; }
        public bool Datewise { get; set; }
    }

    public class DoctorBillMetaData
    {
        [Display(Name = "Visit Charge")]
        public decimal VisitCharge { get; set; }
        [Display(Name = "Consultant Charge")]
        public decimal ConsultantCharge { get; set; }
        [Display(Name = "Operation Charge")]
        public decimal OperationCharge { get; set; }
        [Display(Name = "Other Charge")]
        public decimal OtherChargeAmount { get; set; }
        [Display(Name = "OtherCharge Details")]
        public string OtherChargeDetails { get; set; }
        [Display(Name = "Created On")]
        public System.DateTime CreateDate { get; set; }
        [Display(Name = "Advance")]
        public decimal AdvancePayAmount { get; set; }
        [Display(Name = "Total Amt.")]
        public decimal TotalAmount { get; set; }
        [Display(Name = "Paid Amt.")]
        public decimal PaidAmount { get; set; }
        [Display(Name = "Due Amt.")]
        public decimal DuesAmount { get; set; }
        [Display(Name = "Paid By")]
        public long PaidBy { get; set; }
        [Display(Name = "Paid On")]
        public System.DateTime PaidOn { get; set; }
    }

    public class DoctorVisitPaymentMetaData
    {
        [Display(Name = "Visit Amt.")]
        public decimal VisitAmount { get; set; }
        [Display(Name = "Paid Status")]
        public long PaymentStatus { get; set; }
        [Display(Name = "Payment Received By")]
        public long PaymentReceivedBy { get; set; }
        [Display(Name = "Payment Date")]
        public System.DateTime PaymentReceivedDate { get; set; }
        [Display(Name = "Discount")]
        public decimal DiscountAmount { get; set; }
        [Display(Name = "Discounted By")]
        public long DiscountedBy { get; set; }
        [Display(Name = "Discount Description")]
        public string DiscountDescription { get; set; }
        [Display(Name = "Payment Details")]
        public string PaymentDetails { get; set; }
    }

    public class MedicalPaymentMetaData
    {
        [Display(Name = "Medical Amt.")]
        public decimal MedicalAmount { get; set; }
        [Display(Name = "Status")]
        public long PaymentStatus { get; set; }
        [Display(Name = "Payment Received By")]
        public long PaymentReceivedBy { get; set; }
        [Display(Name = "Payment Date")]
        public System.DateTime PaymentReceivedDate { get; set; }
        [Display(Name = "Discount")]
        public decimal DiscountAmount { get; set; }
        [Display(Name = "Discounted By")]
        public long DiscountedBy { get; set; }
        [Display(Name = "Discount Descr.")]
        public string DiscountDescription { get; set; }
        [Display(Name = "Payment Details")]
        public string PaymentDetails { get; set; }
    }

    public class PatientBillMetaData
    {
        [Display(Name = "Service Amt.")]
        public decimal ServiceAmount { get; set; }
        [Display(Name = "Lab Amt.")]
        public decimal LabAmount { get; set; }
        [Display(Name = "Ward Amt.")]
        public decimal WardAmount { get; set; }
        [Display(Name = "Medical Amt.")]
        public decimal MedicalAmount { get; set; }
        [Display(Name = "Reg. Amt.")]
        public decimal RegistrationAmount { get; set; }
        [Display(Name = "Doctor Visit Amt.")]
        public decimal DoctorVisitAmount { get; set; }
        [Display(Name = "Other Charges")]
        public decimal OtherChargeAmount { get; set; }
        [Display(Name = "Total")]
        public decimal TotalAmount { get; set; }
        [Display(Name = "Advance")]
        public decimal AdvancePaymentAmount { get; set; }
        [Display(Name = "Balance Amt.")]
        public decimal BalanceAmount { get; set; }
        [Display(Name = "Payment Amt.")]
        public decimal PaymentAmount { get; set; }
        [Display(Name = "Due Amt.")]
        public decimal DuesAmount { get; set; }
    }

    public class RegistrationPaymentMetaData
    {
        [Display(Name = "Reg Amt.")]
        public decimal RegistrationAmt { get; set; }
        [Display(Name = "Status")]
        public long PaymentStatus { get; set; }
        [Display(Name = "Payment Received By")]
        public long PaymentReceivedBy { get; set; }
        [Display(Name = "Payment Date")]
        public System.DateTime PaymentReceivedDate { get; set; }
        [Display(Name = "Discount")]
        public decimal DiscountAmount { get; set; }
        [Display(Name = "Discounted By")]
        public long DiscountedBy { get; set; }
        [Display(Name = "Discount Descr.")]
        public string DiscountDescription { get; set; }
        [Display(Name = "Details")]
        public string PaymentDetails { get; set; }
    }

    public class RoomDetailMetaData
    {
        [Display(Name = "Details")]
        public string Details { get; set; }
        [Required]
        [Display(Name = "No. Of Beds")]
        public int TotalBed { get; set; }
        [Required]
        [Display(Name = "Room No")]
        public string RoomNo { get; set; }
        [Required]
        [Display(Name = "Cost")]
        public decimal RoomCost { get; set; }
    }

    //public class ServiceChargeMetaData
    //{
    //    [Required]
    //    [Display(Name = "Date Of Service")]
    //    public System.DateTime ServeDate { get; set; }
    //    [Required]
    //    [Display(Name = "Service Charge")]
    //    public decimal ServiceCharge { get; set; }
    //    [Display(Name = "Service By")]
    //    public long ServeBy { get; set; }
    //    [Display(Name = "Other Details")]
    //    public string OtherDetails { get; set; }
    //    [Display(Name = "Other Charges")]
    //    public decimal OtherCharge { get; set; }
    //    [Display(Name = "Discount")]
    //    public decimal DiscountAmount { get; set; }
    //    [Display(Name = "Discounted By")]
    //    public long DiscountedBy { get; set; }
    //    [Display(Name = "Discount Reason")]
    //    public string DiscountReason { get; set; }
    //}

    public class ServicePaymentMetaData
    {

        [Display(Name = "Service Amt.")]
        public decimal ServiceAmount { get; set; }
        [Display(Name = "Status")]
        public int PaymentStatus { get; set; }
        [Display(Name = "Payment Received By")]
        public long PaymentReceivedBy { get; set; }
        [Display(Name = "Payment Date")]
        public System.DateTime PaymentReceivedDate { get; set; }
        [Display(Name = "Discount")]
        public decimal DiscountAmount { get; set; }
        [Display(Name = "Discounted By")]
        public long DiscountedBy { get; set; }
        [Display(Name = "Discount Descr")]
        public string DiscountDescription { get; set; }
        [Display(Name = "Remark")]
        public string PaymentDetails { get; set; }
    }

    public class ServiceSubCatMetaData
    {
        [Required]
        [Display(Name = "Service Charge")]
        public decimal ServiceCharges { get; set; }
        [Required]
        [Display(Name = "Avg. Duration")]
        public int AverageDuration { get; set; }
    }

    public class VisitReasonMetaData
    {
        [Required]
        [Display(Name = "Visit Purpose")]
        public string VisitPurpose { get; set; }
    }

    public class WarOrRoomChargeMetaData
    {
        [Required]
        [Display(Name = "Start Date")]
        public System.DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public System.DateTime EndDate { get; set; }
        [Required]
        [Display(Name = "Room Charge")]
        public decimal RoomCharge { get; set; }
        [Display(Name = "Other Details")]
        public string OtherDetails { get; set; }
        [Display(Name = "Other Charge")]
        public decimal OtherCharge { get; set; }
        [Display(Name = "Discount Amt.")]
        public decimal DiscountAmount { get; set; }
        [Display(Name = "Discounted By")]
        public long DiscountedBy { get; set; }
        [Display(Name = "Discount Reason")]
        public string DiscountReason { get; set; }

    }

    public class BranchMetaData
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]        
        [Display(Name = "Pri. Phone")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Invalid Phone No")]
        public long PhoneNoPri { get; set; }
        [Display(Name = "Sec. Phone")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Invalid Phone No")]
        public long PhoneNoSec { get; set; }
        [Display(Name = "Mobile")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Enter 10 digit mobile no.")]
        public long MobileNo { get; set; }
        [Display(Name = "Fax No.")]
        public long FaxNo { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        public string EmailInfo { get; set; }
        [Display(Name = "CustomerCare Email")]
        [EmailAddress]
        public string EmailCustomerCare { get; set; }
        [Display(Name = "Website")]
        public string WebAddress { get; set; }
        [Required]       
        public Nullable<int> City_ID { get; set; }
        [Required]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Enter 6 digit pincode.")]
        public int Pincode { get; set; }
    }

    public class EmpTypeMetaData
    {
        [Required]
        public string Description { get; set; }
    }

    public class CityMetaData
    {
        [Required]
        [Display(Name = "City")]
        public string name { get; set; }
        [Required]
        [Display(Name = "State")]
        public Nullable<int> State_ID { get; set; }
    }

    public class StateMetaData
    {
        [Required]
        [Display(Name = "State")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Country")]
        public Nullable<int> Country_ID { get; set; }
    }

    public class LabCatMetaData
    {
        [Required]
        public string Name { get; set; }
    }

    public class LabTestMetaData
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Test Cost")]
        public decimal LabTestCost { get; set; }
        [Required]
        public Nullable<int> LabCategory_ID { get; set; }
    }

    public class WardMetaData
    {
        [Required]
        public string Name { get; set; }
        [Display(Name = " Total Bed")]
        public int TotalNoOfBed { get; set; }
        [Display(Name = "Other Details")]
        public string OtherDetails { get; set; }
        [Display(Name = "Branch Details")]
        public int BranchDetail_ID { get; set; }
    }

    public class AppointmentMetadata
    {
        [Required(ErrorMessage = "Enter the Appointment date.")]
        [Display(Name = "Appointment Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime AppointmentDate { get; set; }
        [Required(ErrorMessage = "Doctor field is required.")]
        public long Doctor_ID { get; set; }
        [Required(ErrorMessage = "Department field is required.")]
        public int Specialization_ID { get; set; }
        [Required(ErrorMessage = "Branch is required.")]
        public int BranchDetails_ID { get; set; }
        public long PatientDetails_ID { get; set; }
        [Required(ErrorMessage = "Shift field is required.")]
        public int ShiftType_ID { get; set; }
        public int PatientType_ID { get; set; }
        [Required(ErrorMessage = "IsReferal is required.")]
        public int IsReferal { get; set; }
    }

    public class PatientVisitMetadata
    {
        [Display(Name = "Visit Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime VisitedDate { get; set; }
        [Required]
        [Display(Name = "Status")]
        public int PatientStatus { get; set; }
        [Required]
        [Display(Name = "Reg. Amt")]
        public decimal RegistrationAmount { get; set; }
        [Display(Name = "Discount")]
        public decimal DiscountAmount { get; set; }
        [Required]
        [Display(Name = "Paid Amt")]
        public decimal PayAmount { get; set; }
        [Display(Name = "Created By")]
        public long CreatedBy { get; set; }
    }
}

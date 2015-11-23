using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Entity
{
    class ValidationClass
    {
    }

    [MetadataType(typeof(EmployeeMetaData))]
    public partial class EmployeeDetail
    {
    }

    [MetadataType(typeof(PatientMetaData))]
    public partial class PatientDetail
    {
    }

    //[MetadataType(typeof(PatientStatusMetaData))]
    //public partial class patientStatu
    //{
    //}

    [MetadataType(typeof(DoctorMetaData))]
    public partial class Doctor
    {
    }

    [MetadataType(typeof(DoctorBillMetaData))]
    public partial class DoctorBillDetail
    {
    }

    [MetadataType(typeof(DoctorVisitPaymentMetaData))]
    public partial class DoctorVisitPaymentDetail
    {
    }

    [MetadataType(typeof(MedicalPaymentMetaData))]
    public partial class MedicalPaymentDetail
    {
    }

    [MetadataType(typeof(PatientBillMetaData))]
    public partial class PatientBill
    {
    }

    [MetadataType(typeof(RoomDetailMetaData))]
    public partial class RoomDetail
    {
    }

    //[MetadataType(typeof(ServiceChargeMetaData))]
    //public partial class ServiceChargeDetail
    //{
    //}

    [MetadataType(typeof(ServicePaymentMetaData))]
    public partial class ServicePaymentDetail
    {
    }

    [MetadataType(typeof(ServiceSubCatMetaData))]
    public partial class ServiceSubCategory
    {
    }

    [MetadataType(typeof(VisitReasonMetaData))]
    public partial class VisitReason
    {
    }

    [MetadataType(typeof(WarOrRoomChargeMetaData))]
    public partial class WardOrRoomChargeDetail
    {
    }

    [MetadataType(typeof(BranchMetaData))]
    public partial class BranchDetail
    { }

    [MetadataType(typeof(EmpTypeMetaData))]
    public partial class EmployeeType
    { }

    [MetadataType(typeof(CityMetaData))]
    public partial class City
    { }

    [MetadataType(typeof(StateMetaData))]
    public partial class State
    { }

    [MetadataType(typeof(LabCatMetaData))]
    public partial class LabCategory
    { }

    [MetadataType(typeof(LabTestMetaData))]
    public partial class LabTest
    { }

    [MetadataType(typeof(WardMetaData))]
    public partial class Ward
    { }

    [MetadataType(typeof(AppointmentMetadata))]
    public partial class Appointment
    { }

    [MetadataType(typeof(PatientVisitMetadata))]
    public partial class PatientVisit
    { }
}

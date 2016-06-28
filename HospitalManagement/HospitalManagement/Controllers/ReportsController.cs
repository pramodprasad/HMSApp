using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using HMS.BAL;
using HMS.Entity;

namespace HospitalManagement.Controllers
{
    public class ReportsController : Controller
    {
        //
        // GET: /Reports/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetBranchDetailsCR()
        {
            DataTable dt = new DataTable();
            dt = ReportsManager.GetBranchDetails(1);

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath("../Content/cr_BranchDetails.rpt");
            rptH.Load();
            rptH.SetDataSource(dt);
            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult GetReceipt(int id)
        {
            DataSet ds = new DataSet();
            ds = ReportsManager.GetReceipt(id, 1);

            ReportClass rptH = new ReportClass();
            //rptH.FileName = Server.MapPath("../Content/cr_RegReceipt.rpt");
            rptH.FileName = @"C:/Users/tanmay/Documents/GitHub/HMSApp/HospitalManagement/HospitalManagement/Content/cr_RegReceipt.rpt";
            rptH.Load();
            //rptH.SetDataSource(ds.Tables["dtpatientvisit"]);
            rptH.SetDataSource(ds.Tables["Table1"]);
            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult AssignValueinReport(int id)
        {
            DataTable dtbranchdetails = new DataTable();
            DataTable dtpatientvisit = new DataTable();
            using (HMSTEntities context = new HMSTEntities())
            {
                var branchdetails = context.sp_BranchDetails(1);
                var patientvisit = context.sp_GetReceipt(id);
                dtbranchdetails = ExtensionMethods.ConvertToDataTable(branchdetails);
                dtpatientvisit = ExtensionMethods.ConvertToDataTable(patientvisit);
                ReportClass rptH = new ReportClass();
                //rptH.FileName = Server.MapPath("~/Content/cr_RegReceipt.rpt");
                rptH.FileName = @"C:/Users/tanmay/Documents/GitHub/HMSApp/HospitalManagement/HospitalManagement/Content/cr_RegReceipt.rpt";
                rptH.Load();
                rptH.Subreports["cr_BranchDetails.rpt"].SetDataSource(dtbranchdetails);//datasource for subreport
                rptH.SetDataSource(dtpatientvisit);//Mainreport datasourcc               
                Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }

        public ActionResult GetPrescriptionReport(int id)
        {
            DataTable dtbranchdetails = new DataTable();
            DataTable dtpatientvisit = new DataTable();
            using (HMSTEntities context = new HMSTEntities())
            {
                var branchdetails = context.sp_BranchDetails(1);
                var patientvisit = context.sp_GetPrescription(id);
                dtbranchdetails = ExtensionMethods.ConvertToDataTable(branchdetails);
                dtpatientvisit = ExtensionMethods.ConvertToDataTable(patientvisit);
                ReportClass rptH = new ReportClass();
                //rptH.FileName = Server.MapPath("~/Content/cr_Prescription.rpt");
                rptH.FileName = @"C:/Users/tanmay/Documents/GitHub/HMSApp/HospitalManagement/HospitalManagement/Content/cr_Prescription.rpt";
                rptH.Load();
                rptH.Subreports["cr_BranchDetails.rpt"].SetDataSource(dtbranchdetails);//datasource for subreport
                rptH.SetDataSource(dtpatientvisit);//Mainreport datasourcc       
                rptH.VerifyDatabase();
                Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }

        public ActionResult GetTodayPatientVisitReport()
        {
            DataTable dtbranchdetails = new DataTable();
            DataTable dtpatientvisit = new DataTable();
            using (HMSTEntities context = new HMSTEntities())
            {
                var branchdetails = context.sp_BranchDetails(1);
                var patientvisit = context.sp_GetRegistrationPayment(null, null, null, null, null);
                dtbranchdetails = ExtensionMethods.ConvertToDataTable(branchdetails);
                dtpatientvisit = ExtensionMethods.ConvertToDataTable(patientvisit);
                ReportClass rptH = new ReportClass();
                //rptH.FileName = Server.MapPath("~/Content/cr_Prescription.rpt");
                rptH.FileName = @"C:/Users/tanmay/Documents/GitHub/HMSApp/HospitalManagement/HospitalManagement/Content/cr_RegistrationPayment.rpt";
                rptH.Load();
                rptH.Subreports["cr_BranchDetails.rpt"].SetDataSource(dtbranchdetails);//datasource for subreport
                rptH.SetDataSource(dtpatientvisit);//Mainreport datasourcc       
                rptH.VerifyDatabase();
                Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }

       
	}
}
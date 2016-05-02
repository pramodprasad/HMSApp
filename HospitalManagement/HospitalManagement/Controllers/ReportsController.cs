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
            dt = ReportsManager.GetBranchDetails(2);

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath("../Content/crReportHeader.rpt");
            rptH.Load();
            rptH.SetDataSource(dt);
            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
	}
}
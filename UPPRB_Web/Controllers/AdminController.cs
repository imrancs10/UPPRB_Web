using UPPRB_Web.BAL.Commom;
using UPPRB_Web.Global;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace UPPRB_Web.Controllers
{
    public class AdminController : CommonController
    {
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult NoticeEntry()
        {
            return View();
        }



        [HttpPost]
        public ActionResult SaveDoctorType(int doctor, int doctortype)
        {
            //DoctorDetails _details = new DoctorDetails();
            //var result = _details.UpdateDoctorType(doctor, doctortype);
            //if (result == Enums.CrudStatus.Updated)
            //    SetAlertMessage("Doctor Type saved.");
            //else
            //    SetAlertMessage("Doctor Type not saved.");
            return RedirectToAction("DoctorType");
        }

        public ActionResult PatientBillReport()
        {
            return View();
        }

        public ActionResult PatientLabReport()
        {
            return View();
        }

        [HttpPost]
        //HttpPostedFileBase reportfile,
        public ActionResult SetBillingReport(int PatientId, string BillNo, string BillType, DateTime BillDate, string ReportUrl, decimal BillAmount, string BillID)
        {
            string ReportPath = string.Empty;
            //if (reportfile != null)
            //{
            //    CommonDetails fileupload = new CommonDetails();
            //    ReportPath = fileupload.ReportFileUpload(reportfile, Global.Enums.ReportType.Bill, BillNo);
            //}
            //else
            //{
            //    ReportPath = string.Empty;
            //}
            //ReportPath = string.Empty;
            //ReportDetails _details = new ReportDetails();
            //_details.SetBillReportData(PatientId, BillNo, BillType, BillDate, ReportPath, BillAmount, BillID);
            return View("PatientBillReport");
        }

        [HttpPost]
        public ActionResult SetLabReport(HttpPostedFileBase reportfile, DateTime ReportDate, int PatientId, string BillNo, string RefNo, string LabName, string ReportUrl, string doctorId)
        {
            string ReportPath = string.Empty;
            //if (reportfile != null)
            //{
            //    CommonDetails fileupload = new CommonDetails();
            //    ReportPath = fileupload.ReportFileUpload(reportfile, Global.Enums.ReportType.Lab, RefNo);
            //}
            //else
            //{
            //    ReportPath = ReportUrl;
            //}
            //ReportDetails _details = new ReportDetails();
            //_details.SetLabReportData(PatientId, BillNo, RefNo, ReportPath, LabName, ReportDate, doctorId);
            return View("PatientLabReport");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
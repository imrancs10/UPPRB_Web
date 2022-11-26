using UPPRB_Web.BAL.Commom;
using UPPRB_Web.Global;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;
using Org.BouncyCastle.Asn1.X509;
using DataLayer;
using UPPRB_Web.BAL.Masters;

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
        public ActionResult NoticeEntry(HttpPostedFileBase postedFile, string NoticeType, string NoticeCategory, string NoticeSubCategory, string Subject, string NoticeDate, string fileURL)
        {
            string filename = postedFile != null ? postedFile.FileName.Substring(0, postedFile.FileName.LastIndexOf('.')) + Guid.NewGuid().ToString() + "." + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.') + 1, postedFile.FileName.Length - postedFile.FileName.LastIndexOf('.') - 1) : null;
            Notice notice = new Notice()
            {
                CreatedBy = UserData.UserId,
                CreatedDate = DateTime.Today,
                filename = filename,
                fileURL = fileURL,
                NoticeCategoryId = Convert.ToInt32(NoticeCategory),
                NoticeDate = Convert.ToDateTime(NoticeDate),
                NoticeSubCategoryId = Convert.ToInt32(NoticeSubCategory),
                NoticeType = Convert.ToInt32(NoticeType),
                Subject = Subject
            };
            AdminDetails detail = new AdminDetails();
            var saveStatus = detail.SaveNotice(notice);
            if (saveStatus == Enums.CrudStatus.Saved)
            {
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/FilesUploaded/Notices/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    postedFile.SaveAs(path + Path.GetFileName(filename));
                }
            }
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
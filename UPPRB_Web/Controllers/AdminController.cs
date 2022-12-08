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
using UPPRB_Web.Infrastructure.Authentication;
using static iTextSharp.tool.xml.html.HTML;

namespace UPPRB_Web.Controllers
{
    [CustomAuthorize]
    public class AdminController : CommonController
    {
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult NoticeEntry(bool? deleteMessage)
        {
            //var detail = new GeneralDetails();
            //var allnotice = detail.GetNoticeDetail();
            //ViewData["NoticeData"] = allnotice;
            if (deleteMessage == true)
            {
                SetAlertMessage("Upload Data has been Deleted", "Notice Entry");
            }
            return View();
        }
        [HttpPost]
        public JsonResult GetAllNotice()
        {
            var detail = new GeneralDetails();
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            string filterText = Request["search[value]"];
            var result = detail.GetNoticeDetail();

            if (!string.IsNullOrEmpty(filterText))
            {
                result = result.Where(x => x.EntryTypeName.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.NoticeTypeName.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.NoticeCategoryName.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            recordsTotal = result.Count();
            var data = result.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteNotice(int Id)
        {
            var detail = new GeneralDetails();
            var result = detail.DeleteNotice(Id);
            return RedirectToAction("NoticeEntry", new { deleteMessage = true });
        }

        [HttpPost]
        public ActionResult NoticeEntry(HttpPostedFileBase postedFile, string NoticeType, string NoticeCategory, string EntryType, string Subject, string NoticeDate, string fileURL, string highlightNew, string EntryTypeName)
        {
            string filename = postedFile != null ? postedFile.FileName.Substring(0, postedFile.FileName.LastIndexOf('.')) + Guid.NewGuid().ToString() + "." + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.') + 1, postedFile.FileName.Length - postedFile.FileName.LastIndexOf('.') - 1) : null;
            Notice notice = new Notice()
            {
                CreatedBy = UserData.UserId,
                CreatedDate = DateTime.Today,
                filename = filename,
                fileURL = fileURL,
                NoticeCategoryId = !string.IsNullOrEmpty(NoticeCategory) ? (int?)Convert.ToInt32(NoticeCategory) : null,
                NoticeDate = Convert.ToDateTime(NoticeDate),
                EntryTypeId = !string.IsNullOrEmpty(EntryType) ? (int?)Convert.ToInt32(EntryType) : null,
                NoticeType = !string.IsNullOrEmpty(NoticeType) ? (int?)Convert.ToInt32(NoticeType) : null,
                Subject = Subject,
                IsNew = highlightNew == "on" ? true : false
            };
            AdminDetails detail = new AdminDetails();
            var saveStatus = detail.SaveNotice(notice);
            if (saveStatus == Enums.CrudStatus.Saved)
            {
                if (postedFile != null)
                {

                    string path = string.Empty;
                    if (!string.IsNullOrEmpty(EntryTypeName))
                        path = Server.MapPath("~/FilesUploaded/" + EntryTypeName + "/");
                    else
                        path = Server.MapPath("~/FilesUploaded/Other/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    postedFile.SaveAs(path + Path.GetFileName(filename));
                }
            }
            SetAlertMessage("Notice Saved", "Success");
            return View();
        }
        public ActionResult AddNotice()
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
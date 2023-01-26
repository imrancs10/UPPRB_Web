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
using Newtonsoft.Json.Linq;

namespace UPPRB_Web.Controllers
{
    [CustomAuthorize]
    public class AdminController : CommonController
    {
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult NoticeEntry(int? noticeId)
        {
            //var detail = new GeneralDetails();
            //var allnotice = detail.GetNoticeDetail();
            //ViewData["NoticeData"] = allnotice;
            //if (deleteMessage == true)
            //{
            //    SetAlertMessage("Upload Data has been Deleted", "Notice Entry");
            //}
            return View();
        }
        [HttpPost]
        public JsonResult GetNoticeForEdit(int? noticeId = null)
        {
            var detail = new GeneralDetails();
            var result = detail.GetNoticeDetail(null, null, null, noticeId).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NoticeEntryList(int entryTypeId)
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllNotice(int? entryTypeId = null)
        {
            var detail = new GeneralDetails();
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            string filterText = Request["search[value]"];
            var result = detail.GetNoticeDetail(null, null, entryTypeId);

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
        public ActionResult NoticeEntry(HttpPostedFileBase postedFile, string NoticeType, string NoticeCategory,
            string EntryType, string Subject, string NoticeDate, string fileURL, string highlightNew,
            string EntryTypeName, string hiddenNoticeID)
        {
            string filename = postedFile != null ? postedFile.FileName.Substring(0, postedFile.FileName.LastIndexOf('.')) + Guid.NewGuid().ToString() + "." + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.') + 1, postedFile.FileName.Length - postedFile.FileName.LastIndexOf('.') - 1) : null;
            Notice notice = new Notice()
            {
                Id = !string.IsNullOrEmpty(hiddenNoticeID) ? Convert.ToInt32(hiddenNoticeID) : 0,
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
            if (saveStatus == Enums.CrudStatus.Saved || saveStatus == Enums.CrudStatus.Updated)
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

        public ActionResult EnquiryList()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllEnquiry()
        {
            var detail = new GeneralDetails();
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            string filterText = Request["search[value]"];
            var result = detail.GetAllEnquiry();

            if (!string.IsNullOrEmpty(filterText))
            {
                result = result.Where(x => x.Subject.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.Message.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.Name.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            recordsTotal = result.Count();
            var data = result.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FeedbackList()
        {
            return View();
        }
        public ActionResult PACEntry()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PACEntry(string hiddenId,HttpPostedFileBase postedFile, string State, string Zone,
       string Range, string District, string PoliceStation, string ExamineCenterName, string Address, string FIRNo, string FIRDate, string PublishDate, string AccusedName, string FIRDetails, string fileURL)
        {
            State = "1";
            string filename = postedFile != null ? postedFile.FileName.Substring(0, postedFile.FileName.LastIndexOf('.')) + Guid.NewGuid().ToString() + "." + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.') + 1, postedFile.FileName.Length - postedFile.FileName.LastIndexOf('.') - 1) : null;
            PACEntry notice = new PACEntry()
            {
                Id = !string.IsNullOrEmpty(hiddenId) ? Convert.ToInt32(hiddenId) : 0,
                CreatedDate = DateTime.Today,
                FileUploadName = filename,
                FileURL = fileURL,
                State_Id = !string.IsNullOrEmpty(State) ? (int?)Convert.ToInt32(State) : null,
                PublishDate = !string.IsNullOrEmpty(PublishDate) ? (DateTime?)Convert.ToDateTime(PublishDate) : null,
                FIRDate = !string.IsNullOrEmpty(FIRDate) ? (DateTime?)Convert.ToDateTime(FIRDate) : null,
                Zone_Id = !string.IsNullOrEmpty(Zone) ? (int?)Convert.ToInt32(Zone) : null,
                Range_Id = !string.IsNullOrEmpty(Range) ? (int?)Convert.ToInt32(Range) : null,
                District_Id = !string.IsNullOrEmpty(District) ? (int?)Convert.ToInt32(District) : null,
                AccusedName = AccusedName,
                Address = Address,
                ExamineCenterName = ExamineCenterName,
                FIRDetails = FIRDetails,
                FIRNo = FIRNo,
                PS_Id = !string.IsNullOrEmpty(PoliceStation) ? (int?)Convert.ToInt32(PoliceStation) : null,

            };
            AdminDetails detail = new AdminDetails();
            //bool isDuplicate = detail.IsDuplicateFIR(notice);
            //if(isDuplicate== false)
            {
                var saveStatus = detail.SavePACEntry(notice);
                if (saveStatus == Enums.CrudStatus.Saved || saveStatus == Enums.CrudStatus.Updated)
                {
                    if (postedFile != null)
                    {

                        string path = Server.MapPath("~/FilesUploaded/PAC/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        postedFile.SaveAs(path + Path.GetFileName(filename));
                    }
                }
                SetAlertMessage("PAC Enrty Saved", "Success");
            }
            //else
            //{
            //    SetAlertMessage("PAC Enrty is duplicate", "Failed");
            //}

            return View();
        }

        public ActionResult PACList()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetPACForEdit(int Id)
        {
            var detail = new GeneralDetails();
            var result = detail.GetAllPACDetail(Id).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePACEntry(int Id)
        {
            var detail = new GeneralDetails();
            var result = detail.DeletePACEntry(Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult PromotionEntry()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PromotionEntry(HttpPostedFileBase postedFile, string Subject, string fileURL, string promotionType)
        {
            string filename = postedFile != null ? postedFile.FileName.Substring(0, postedFile.FileName.LastIndexOf('.')) + Guid.NewGuid().ToString() + "." + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.') + 1, postedFile.FileName.Length - postedFile.FileName.LastIndexOf('.') - 1) : null;
            PromotionDetail notice = new PromotionDetail()
            {
                UpdatedDate = DateTime.Today,
                FileName = filename,
                FIleURL = fileURL,
                Parent_Id = !string.IsNullOrEmpty(promotionType) ? (int?)Convert.ToInt32(promotionType) : null,
                Subject = Subject
            };
            AdminDetails detail = new AdminDetails();
            var saveStatus = detail.SavePromotionEntry(notice);
            if (saveStatus == Enums.CrudStatus.Saved || saveStatus == Enums.CrudStatus.Updated)
            {
                if (postedFile != null)
                {

                    string path = Server.MapPath("~/FilesUploaded/Promotion/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    postedFile.SaveAs(path + Path.GetFileName(filename));
                }
            }
            SetAlertMessage("Promotion Entry Saved", "Success");
            return View();
        }
        public ActionResult AddPoliceStation()
        {
            return View();
        }
        public ActionResult DirectRecruitementEntry()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DirectRecruitementEntry(HttpPostedFileBase postedFile, string Subject, string fileURL, string promotionType)
        {
            string filename = postedFile != null ? postedFile.FileName.Substring(0, postedFile.FileName.LastIndexOf('.')) + Guid.NewGuid().ToString() + "." + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.') + 1, postedFile.FileName.Length - postedFile.FileName.LastIndexOf('.') - 1) : null;
            DirectRecruitementDetail notice = new DirectRecruitementDetail()
            {
                UpdatedDate = DateTime.Today,
                FileName = filename,
                FIleURL = fileURL,
                Parent_Id = !string.IsNullOrEmpty(promotionType) ? (int?)Convert.ToInt32(promotionType) : null,
                Subject = Subject
            };
            AdminDetails detail = new AdminDetails();
            var saveStatus = detail.SaveDirectRecruitementEntry(notice);
            if (saveStatus == Enums.CrudStatus.Saved || saveStatus == Enums.CrudStatus.Updated)
            {
                if (postedFile != null)
                {

                    string path = Server.MapPath("~/FilesUploaded/DirectRecruitement/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    postedFile.SaveAs(path + Path.GetFileName(filename));
                }
            }
            SetAlertMessage("Direct Recruitement Entry Saved", "Success");
            return View();
        }


        [HttpPost]
        public JsonResult GetAllFeedback()
        {
            var detail = new GeneralDetails();
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            string filterText = Request["search[value]"];
            var result = detail.GetAllFeedback();

            if (!string.IsNullOrEmpty(filterText))
            {
                result = result.Where(x => x.Subject.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.Message.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.Name.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            recordsTotal = result.Count();
            var data = result.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
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
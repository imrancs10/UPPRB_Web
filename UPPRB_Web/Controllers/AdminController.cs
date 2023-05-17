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
using System.Xml.Linq;
using UPPRB_Web.BAL.Login;
using System.Text.RegularExpressions;
using Microsoft.Ajax.Utilities;
using System.Threading.Tasks;
using UPPRB_Web.Infrastructure;
using UPPRB_Web.Infrastructure.Utility;
using Org.BouncyCastle.Asn1.Ocsp;

namespace UPPRB_Web.Controllers
{
    [CustomAuthorize]
    [UserValidate]
    public class AdminController : CommonController
    {
        List<string> fileTypeAccepted = new List<string>()
        {
           "pdf","doc", "docx", "xls", "xlsx", "png", "bmp", "jpg", "jpeg"
        };
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult SearchPACList()
        {
            return View();
        }
        public ActionResult SearchSolverList()
        {
            return View();
        }

        public ActionResult SearchBlacklistedList()
        {
            return View();
        }
        public ActionResult CreateUser()
        {
            var detail = new AdminDetails();
            ViewData["UserData"] = detail.GetAdminUser();
            return View();
        }
        [HttpPost]
        public ActionResult CreateUser(string hiddenID, string Role, string UserName,
                                               string Name, string Mobile, string Email)
        {
            AdminUser notice = new AdminUser()
            {
                IsActive = true,
                Id = !string.IsNullOrEmpty(hiddenID) ? Convert.ToInt32(hiddenID) : 0,
                EmailID = Email,
                MobileNumber = Convert.ToInt64(Mobile),
                Name = Name,
                Password = Convert.ToString(ConfigurationManager.AppSettings["DefaultPassword"]),
                RoleId = !string.IsNullOrEmpty(Role) ? (int?)Convert.ToInt32(Role) : null,
                UserName = UserName,
            };
            AdminDetails detail = new AdminDetails();
            var saveStatus = detail.SaveNewUserEntry(notice);
            if (saveStatus == Enums.CrudStatus.Saved || saveStatus == Enums.CrudStatus.Updated)
                SetAlertMessage("User Created", "Success");
            else
                SetAlertMessage("User not Created", "Fail");

            return RedirectToAction("CreateUser");
        }
        [HttpPost]
        public JsonResult DeleteUserDetailEntry(int Id)
        {
            var detail = new AdminDetails();
            var result = detail.DeleteUserDetailEntry(Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddMedalDetails()
        {
            var detail = new AdminDetails();
            ViewData["MedalData"] = detail.GetMedalEntry();
            return View();
        }

        [HttpPost]
        public ActionResult AddMedalDetails(string hiddenID, HttpPostedFileBase postedFile, string MedalCategory, string GivenBY,
                                                string ToWhom, string MedalDescription, string MedalDate)
        {
            string filename = postedFile != null ? postedFile.FileName.Substring(0, postedFile.FileName.LastIndexOf('.')) + Guid.NewGuid().ToString() + "." + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.') + 1, postedFile.FileName.Length - postedFile.FileName.LastIndexOf('.') - 1) : null;
            MedalDetail notice = new MedalDetail()
            {
                IsActive = true,
                Id = !string.IsNullOrEmpty(hiddenID) ? Convert.ToInt32(hiddenID) : 0,
                UpdatedDate = DateTime.Today,
                FileName = filename,
                MedalCategoryId = !string.IsNullOrEmpty(MedalCategory) ? (int?)Convert.ToInt32(MedalCategory) : null,
                MedalGivenDate = !string.IsNullOrEmpty(MedalDate) ? (DateTime?)Convert.ToDateTime(MedalDate) : null,
                GivenBy = GivenBY,
                ToWhom = ToWhom,
                MedalDescription = MedalDescription
            };
            AdminDetails detail = new AdminDetails();
            var saveStatus = detail.SaveMedalEntry(notice);
            if (saveStatus == Enums.CrudStatus.Saved || saveStatus == Enums.CrudStatus.Updated)
            {
                if (postedFile != null)
                {

                    string path = Server.MapPath("~/FilesUploaded/Medal/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    postedFile.SaveAs(path + Path.GetFileName(filename));
                }
            }
            SetAlertMessage("Medal Enrty Saved", "Success");

            return RedirectToAction("AddMedalDetails");
        }
        [HttpPost]
        public JsonResult DeleteMedalDetailEntry(int Id)
        {
            var detail = new AdminDetails();
            var result = detail.DeleteMedalDetailEntry(Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEventCalender()
        {
            var detail = new AdminDetails();
            ViewData["EventData"] = detail.GetEventCalender();
            return View();
        }

        [HttpPost]
        public ActionResult AddEventCalender(string hiddenID, HttpPostedFileBase postedFile, string EventTitle, string EventDescription,
                                               string EventDate)
        {
            string filename = postedFile != null ? postedFile.FileName.Substring(0, postedFile.FileName.LastIndexOf('.')) + Guid.NewGuid().ToString() + "." + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.') + 1, postedFile.FileName.Length - postedFile.FileName.LastIndexOf('.') - 1) : null;
            EventCalender notice = new EventCalender()
            {
                IsActive = true,
                Id = !string.IsNullOrEmpty(hiddenID) ? Convert.ToInt32(hiddenID) : 0,
                UpdatedDate = DateTime.Today,
                FileName = filename,
                EventDate = !string.IsNullOrEmpty(EventDate) ? (DateTime?)Convert.ToDateTime(EventDate) : null,
                EventTitle = EventTitle,
                EventDescription = EventDescription
            };
            AdminDetails detail = new AdminDetails();
            var saveStatus = detail.SaveEventCalender(notice);
            if (saveStatus == Enums.CrudStatus.Saved || saveStatus == Enums.CrudStatus.Updated)
            {
                if (postedFile != null)
                {

                    string path = Server.MapPath("~/FilesUploaded/EventCalender/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    postedFile.SaveAs(path + Path.GetFileName(filename));
                }
            }
            SetAlertMessage("Event Calender Saved", "Success");

            return RedirectToAction("AddEventCalender");
        }
        [HttpPost]
        public JsonResult DeleteEventCalender(int Id)
        {
            var detail = new AdminDetails();
            var result = detail.DeleteEventCalender(Id);
            return Json(result, JsonRequestBehavior.AllowGet);
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
            var fileExtension = filename != null ? filename.Substring(filename.LastIndexOf('.') + 1, filename.Length - filename.LastIndexOf('.') - 1) : null;
            if (postedFile != null && !fileTypeAccepted.Contains(fileExtension))
            {
                SetAlertMessage("Uploaded file is not accepted, please choose correct file type", "fail");
                return View();
            }
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
        public ActionResult AddFAQ()
        {
            var detail = new AdminDetails();
            ViewData["FAQData"] = detail.GetFAQEntry();
            return View();
        }
        [HttpPost]
        public ActionResult AddFAQ(string Question, string Answer, string hiddenID)
        {
            FAQDetail notice = new FAQDetail()
            {
                Id = !string.IsNullOrEmpty(hiddenID) ? Convert.ToInt32(hiddenID) : 0,
                FAQ_Question = Question,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                FAQ_Answer = Answer,
            };
            AdminDetails detail = new AdminDetails();
            var saveStatus = detail.SaveFAQEntry(notice);
            if (saveStatus == Enums.CrudStatus.Saved || saveStatus == Enums.CrudStatus.Updated)
            {
                SetAlertMessage("FAQ Entry Saved", "Success");
            }
            else
            {
                SetAlertMessage("FAQ Entry Failed", "Error");

            }
            return RedirectToAction("AddFAQ");
        }
        [HttpPost]
        public JsonResult DeleteFAQEntry(int Id)
        {
            var detail = new AdminDetails();
            var result = detail.DeleteFAQEntry(Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddPSDetails()
        {
            var detail = new AdminDetails();
            ViewData["PSData"] = detail.GetPSEntry();
            return View();
        }
        [HttpPost]
        public ActionResult AddPSDetails(string District, string PSName, string hiddenID)
        {
            PSMaster notice = new PSMaster()
            {
                PSId = !string.IsNullOrEmpty(hiddenID) ? Convert.ToInt32(hiddenID) : 0,
                PSName = PSName,
                DistrictId = !string.IsNullOrEmpty(District) ? (int?)Convert.ToInt32(District) : null,
            };
            AdminDetails detail = new AdminDetails();
            var saveStatus = detail.SavePSEntry(notice);
            if (saveStatus == Enums.CrudStatus.Saved || saveStatus == Enums.CrudStatus.Updated)
            {
                SetAlertMessage("PS Entry Saved", "Success");
            }
            else
            {
                SetAlertMessage("PS Entry Failed", "Error");

            }
            return RedirectToAction("AddPSDetails");
        }
        [HttpPost]
        public JsonResult DeletePSEntry(int Id)
        {
            var detail = new AdminDetails();
            var result = detail.DeletePSEntry(Id);
            return Json(result, JsonRequestBehavior.AllowGet);
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
        public ActionResult EnquireySendEmail(int Id)
        {
            var detail = new GeneralDetails();
            var result = detail.GetEnquiryById(Id);
            ViewData["EquiryData"] = result;
            ViewData["Id"] = Id;
            return View();
        }
        [HttpPost]
        public ActionResult EnquiryResponseSubmit(int Id)
        {
            var detail = new GeneralDetails();
            HttpRequestBase request = ControllerContext.HttpContext.Request;
            string EmailResponse = request.Unvalidated.Form.Get("EmailResponse");
            var result = detail.GetEnquiryById(Id);
            SendMailFordeviceVerification(result.Name, result.Email, EmailResponse);
            return RedirectToAction("EnquiryList");
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
        public ActionResult PACEntry(string hiddenId, HttpPostedFileBase postedFile, string State, string Zone,
       string Range, string District, string PoliceStation, string ExamineCenterName, string Solver_Name, string Address, string FIRNo, string FIRDate, string PublishDate, string AccusedName, string FIRDetails, string fileURL,string RecruitementType)
        {
            State = "1";
            string filename = postedFile != null ? postedFile.FileName.Substring(0, postedFile.FileName.LastIndexOf('.')) + Guid.NewGuid().ToString() + "." + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.') + 1, postedFile.FileName.Length - postedFile.FileName.LastIndexOf('.') - 1) : null;
            PACEntry notice = new PACEntry()
            {
                IsDeleted = false,
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
                recruitement_type= !string.IsNullOrEmpty(RecruitementType) ? (int?)Convert.ToInt32(RecruitementType) : null,
                AccusedName = AccusedName,
                Address = Address,
                ExamineCenterName = ExamineCenterName,
                Solver_Name = Solver_Name,
                FIRDetails = FIRDetails,
                FIRNo = FIRNo,
                PS_Id = !string.IsNullOrEmpty(PoliceStation) ? (int?)Convert.ToInt32(PoliceStation) : null,
            };
            AdminDetails detail = new AdminDetails();
            bool isDuplicate = detail.IsDuplicateFIR(notice);
            if (isDuplicate == false)
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
            else
            {
                SetAlertMessage("PAC Enrty is duplicate! please check Distrct, FIR No, Police Station, Examine Center and Accused Name", "Failed");
            }

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
        [HttpPost]
        public JsonResult DeleteNoticeEntry(int Id)
        {
            var detail = new GeneralDetails();
            var result = detail.DeleteNoticeEntry(Id);
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

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            AdminDetails detail = new AdminDetails();
            int marks = GetPasswordStrength(newPassword);
            string status = "";
            switch (marks)
            {
                case 1:
                    status = "Very Week";
                    break;
                case 2:
                    status = "Week";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Very Strong";
                    break;
                default:
                    break;
            }
            if (newPassword.Trim() == oldPassword.Trim())
            {
                SetAlertMessage("Old Password and New Password should not same", "Error");
                return RedirectToAction("ChangePassword");
            }
            else if (newPassword.Trim() != confirmPassword.Trim())
            {
                SetAlertMessage("New Password and Confirm Password are not matched", "Error");
                return RedirectToAction("ChangePassword");
            }
            else if (marks < 4)
            {
                SetAlertMessage("Strong or very Strong Password Required - Current Status is : " + status, "Error");
                return RedirectToAction("ChangePassword");
            }
            var saveStatus = detail.ChangePassword(oldPassword, newPassword, confirmPassword);
            if (saveStatus == Enums.CrudStatus.Saved || saveStatus == Enums.CrudStatus.Updated)
            {
                SetAlertMessage("Password Changed, Login again with new password", "Success");
                LoginDetails _details = new LoginDetails();
                _details.UpdateLoginDetail();
                return RedirectToAction("ChangePassword");
            }
            else if (saveStatus == Enums.CrudStatus.DataNotFound)
            {
                SetAlertMessage("Old Password is not correct", "Success");
                return RedirectToAction("ChangePassword");
            }
            else
            {
                SetAlertMessage("Some Error Occured", "Error");
                return RedirectToAction("ChangePassword");
            }
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
        private int GetPasswordStrength(string password)
        {
            int Marks = 0;
            // here we will check password strength
            if (password.Length < 6)
            {
                // Very Week
                return 1;
            }
            else
            {
                Marks = 1;
            }
            if (Regex.IsMatch(password, "[a-z]"))
            {
                // 2    week
                Marks++;
            }
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                // 3    medium
                Marks++;
            }
            if (Regex.IsMatch(password, "[0-9]"))
            {
                //4     strong
                Marks++;
            }
            if (Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
            {
                //5     very strong
                Marks++;
            }
            return Marks;

        }
        private async Task SendMailFordeviceVerification(string name, string emailId, string emailResponse)
        {
            await Task.Run(() =>
            {
                //Send Email
                Message msg = new Message()
                {
                    MessageTo = emailId,
                    MessageNameTo = name,
                    Subject = "UPPRBPB | Your Enquiry Response",
                    Body = emailResponse
                };
                ISendMessageStrategy sendMessageStrategy = new SendMessageStrategyForEmail(msg);
                sendMessageStrategy.SendMessages();

                //Send SMS
                //msg.Body = "Hello " + string.Format("{0} {1}", firstname, lastname) + "\nAs you requested, here is a OTP " + verificationCode + " you can use it to verify your mobile number before 15 minutes.\n Regards:\n Patient Portal(RMLHIMS)";
                //msg.MessageTo = mobilenumber;
                //msg.MessageType = MessageType.OTP;
                //sendMessageStrategy = new SendMessageStrategyForSMS(msg);
                //sendMessageStrategy.SendMessages();
            });
        }
        public ActionResult Logout()
        {
            LoginDetails _details = new LoginDetails();
            _details.UpdateLoginDetail();
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
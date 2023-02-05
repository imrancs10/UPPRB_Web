using DataLayer;
using log4net;
using UPPRB_Web.BAL.Lookup;
using UPPRB_Web.Global;
using UPPRB_Web.Infrastructure;
using UPPRB_Web.Infrastructure.Authentication;
using UPPRB_Web.Infrastructure.Utility;
using UPPRB_Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using static UPPRB_Web.Global.Enums;
using UPPRB_Web.BAL.Masters;
using System.Data.Entity.Migrations.Model;
using static iTextSharp.tool.xml.html.HTML;
using UPPRB_Web.BAL.Login;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Data.Entity;

namespace UPPRB_Web.Controllers
{
    public class HomeController : CommonController
    {
        //Declaring Log4Net
        ILog logger = LogManager.GetLogger(typeof(HomeController));
        public ActionResult Index()
        {
            var detail = new GeneralDetails();
            var latestEvent = detail.GetLatestEventDetail();
            ViewData["LatestEvent"] = latestEvent;
            var highlightedNotice = detail.GetHighlightedNoticeDetail();
            ViewData["HighlightedNotice"] = highlightedNotice;
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Notice(int? noticeId = null, int? categoryId = null)
        {
            var detail = new GeneralDetails();
            var currentDate = DateTime.Now;
            var thresoldDate = currentDate.AddMonths(-1);
            var allnotice = detail.GetNoticeDetail(noticeId, categoryId).Where(x => x.EntryTypeName == "Notice" && currentDate >= x.NoticeDate && thresoldDate.Date <= x.NoticeDate.Value.Date).ToList();
            ViewData["NoticeData"] = allnotice;
            var noticeTypeDetail = detail.GetNoticeHirarchyDetail();
            ViewData["NoticeType"] = noticeTypeDetail;
            return View();
        }

        public ActionResult Result(int? noticeId = null, int? categoryId = null)
        {
            var detail = new GeneralDetails();
            var currentDate = DateTime.Now;
            var thresoldDate = currentDate.AddMonths(-1);
            var allnotice = detail.GetNoticeDetail(noticeId, categoryId).Where(x => x.EntryTypeName == "Result" && currentDate >= x.NoticeDate && thresoldDate.Date <= x.NoticeDate.Value.Date).ToList();
            ViewData["NoticeData"] = allnotice;
            var noticeTypeDetail = detail.GetNoticeHirarchyDetail();
            ViewData["NoticeType"] = noticeTypeDetail;
            return View();
        }

        public ActionResult DirectRecruitment(int? drId)
        {
            var detail = new GeneralDetails();
            var noticeTypeDetail = detail.GetRecursiveDirectRecruitmentDetail();
            ViewData["DRType"] = noticeTypeDetail;

            var allnotice = detail.GetDirectRecruitmentDetail(drId).ToList();
            ViewData["DRDetail"] = allnotice;
            return View();
        }
        public ActionResult Promotion(int? promotionId)
        {
            var detail = new GeneralDetails();
            var noticeTypeDetail = detail.GetRecursivePromotionDetail();
            ViewData["PromotionType"] = noticeTypeDetail;

            var allnotice = detail.GetPromotionDetail(promotionId).ToList();
            ViewData["PromotionDetail"] = allnotice;

            return View();
        }
        public ActionResult GovernmentOrders()
        {
            var detail = new GeneralDetails();
            var currentDate = DateTime.Now;
            var thresoldDate = currentDate.AddMonths(-1);
            var allnotice = detail.GetNoticeDetail().Where(x => x.EntryTypeName == "GO" && currentDate >= x.NoticeDate && thresoldDate.Date <= x.NoticeDate.Value.Date).ToList();
            ViewData["NoticeData"] = allnotice;
            var noticeTypeDetail = detail.GetGONoticeHirarchyDetail();
            ViewData["NoticeType"] = noticeTypeDetail;
            return View();
        }
        public ActionResult GovernmentOrders_Detail(int? noticeId = null)
        {
            var detail = new GeneralDetails();
            var currentDate = DateTime.Now;
            var allnotice = detail.GetNoticeDetail(noticeId).Where(x => x.EntryTypeName == "GO" && currentDate >= x.NoticeDate).ToList();
            ViewData["NoticeData"] = allnotice;
            var noticeTypeDetail = detail.GetGONoticeHirarchyDetail();
            ViewData["NoticeType"] = noticeTypeDetail;
            return View();
        }
        public ActionResult SelectionProcedure()
        {
            return View();
        }
        public ActionResult Administration()
        {
            return View();
        }
        public ActionResult PhotoGallery(int? Id = null)
        {
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);

            var detail = new GeneralDetails();
            var allnotice = detail.GetNoticeDetail(Id).Where(x => x.EntryTypeName == "PhotoGalary" && dateTime_Indian >= x.NoticeDate).GroupBy(x => x.Subject).Select(x => x.First()).ToList();
            ViewData["NoticeData"] = allnotice;
            var noticeTypeDetail = detail.GetPhotoGalaryNoticeHirarchyDetail();
            ViewData["NoticeType"] = noticeTypeDetail;
            return View();
        }
        public ActionResult PhotoGalleryDetail(string Subject = null)
        {
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);

            var detail = new GeneralDetails();
            var allnotice = detail.GetNoticeDetail().Where(x => x.EntryTypeName == "PhotoGalary" && dateTime_Indian >= x.NoticeDate && (x.Subject.Trim() == Subject.Trim() || Subject == null)).ToList();
            ViewData["NoticeData"] = allnotice;
            var noticeTypeDetail = detail.GetPhotoGalaryNoticeHirarchyDetail();
            ViewData["NoticeType"] = noticeTypeDetail;
            return View();
        }
        public ActionResult PrevailingRecruitmentProcesses()
        {
            var detail = new GeneralDetails();
            var noticeTypeDetail = detail.GetNoticeHirarchyDetail();
            ViewData["NoticeType"] = noticeTypeDetail;
            return View();
        }
        public ActionResult InfoAboutDirectRecruitment()
        {
            var detail = new GeneralDetails();
            var noticeTypeDetail = detail.GetNoticeHirarchyDetail();
            ViewData["NoticeType"] = noticeTypeDetail;
            return View();
        }
        public ActionResult PrevailingPromotionProcedures()
        {
            var detail = new GeneralDetails();
            var noticeTypeDetail = detail.GetNoticeHirarchyDetail();
            ViewData["NoticeType"] = noticeTypeDetail;
            return View();
        }
        public ActionResult InfoAboutPromotion()
        {
            var detail = new GeneralDetails();
            var noticeTypeDetail = detail.GetNoticeHirarchyDetail();
            ViewData["NoticeType"] = noticeTypeDetail;
            return View();
        }
        public ActionResult FAQ()
        {
            var detail = new AdminDetails();
            ViewData["FAQData"] = detail.GetFAQEntry();
            return View();
        }
        public ActionResult SpecialDates()
        {
            return View();
        }
        public ActionResult Site_Security()
        {
            return View();
        }
        public ActionResult Archiving_Policy()
        {
            return View();
        }
        public ActionResult Hyperlink_Policy()
        {
            return View();
        }
        public ActionResult Privacy_Policy()
        {
            return View();
        }
        public ActionResult Terms_Conditions()
        {
            return View();
        }
        public ActionResult Copyright_Statement()
        {
            return View();
        }
        public ActionResult Disclaimer()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult About_DGMessage()
        {
            return View();
        }
        public ActionResult About_Vision()
        {
            return View();
        }

        public ActionResult About_Logo()
        {
            return View();
        }

        public ActionResult About_OrgStructure()
        {
            return View();
        }

        public ActionResult About_GovernmentOrder()
        {
            return View();
        }
        public ActionResult About_RTI()
        {
            return View();
        }

        public ActionResult HowToReach()
        {
            return View();
        }

        public ActionResult OfficersDirectory()
        {
            return View();
        }

        public ActionResult EnquiryForm()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EnquiryForm(string name, string mobile, string email, string address, string subject, string message)
        {
            Enquiry enquiry = new Enquiry()
            {
                CreatedDate = DateTime.Today,
                Address = address,
                Subject = subject,
                Email = email,
                Message = message,
                Mobile = mobile,
                Name = name
            };
            GeneralDetails detail = new GeneralDetails();
            var saveStatus = detail.SaveEnquiry(enquiry);
            if (saveStatus == Enums.CrudStatus.Saved)
                SetAlertMessage("Enquiry Submitted", "Success");
            else
                SetAlertMessage("Enquiry not Submitted", "Failed");
            return View();
        }
        public ActionResult Feedback()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Feedback(string name, string mobile, string email, string address, string subject, string message)
        {
            Feedback feedback = new Feedback()
            {
                CreatedDate = DateTime.Today,
                Address = address,
                Subject = subject,
                Email = email,
                Message = message,
                Mobile = mobile,
                Name = name
            };
            GeneralDetails detail = new GeneralDetails();
            var saveStatus = detail.SaveFeedback(feedback);
            if (saveStatus == Enums.CrudStatus.Saved)
                SetAlertMessage("Feedback Submitted", "Success");
            else
                SetAlertMessage("Feedback not Submitted", "Failed");
            return View();
        }
        public ActionResult Tender()
        {
            var detail = new GeneralDetails();
            var currentDate = DateTime.Now;
            var thresoldDate = currentDate.AddMonths(-1);
            var allnotice = detail.GetNoticeDetail().Where(x => x.EntryTypeName == "Tender" && currentDate >= x.NoticeDate && thresoldDate.Date <= x.NoticeDate.Value.Date).ToList();
            ViewData["NoticeData"] = allnotice;
            return View();
        }
        public ActionResult PACLogin()
        {
            return View();
        }
        public ActionResult Manual()
        {
            var detail = new GeneralDetails();
            var noticeTypeDetail = detail.GetRecruitmentRuleNoticeHirarchyDetail();
            ViewData["NoticeType"] = noticeTypeDetail;
            return View();
        }
        public ActionResult Manual_Detail(int? noticeId = null, int? categoryId = null)
        {
            var detail = new GeneralDetails();
            var currentDate = DateTime.Now;
            var allnotice = detail.GetNoticeDetail(noticeId, categoryId).Where(x => x.EntryTypeName == "RecruitmentRules" && currentDate >= x.NoticeDate).ToList();
            ViewData["NoticeData"] = allnotice;
            var noticeTypeDetail = detail.GetRecruitmentRuleNoticeHirarchyDetail();
            ViewData["NoticeType"] = noticeTypeDetail;
            return View();
        }
        [HttpPost]
        public ActionResult PACLogin(string username, string password)
        {
            LoginDetails _details = new LoginDetails();
            string _response = string.Empty;
            Enums.LoginMessage message = _details.PACLogin(username, password);
            _response = LoginResponse(message);
            if (message == Enums.LoginMessage.Authenticated)
            {
                setUserClaim();
                return RedirectToAction("PACDocument", "PAC");
            }
            else
            {
                SetAlertMessage(_response, "Login Response");
                return View("PACLogin");
            }
        }

        public ActionResult ImportantCourtDecision(int? noticeId = null, int? categoryId = null)
        {
            var detail = new GeneralDetails();
            var allnotice = detail.GetNoticeDetail(noticeId, categoryId).Where(x => x.EntryTypeName == "Court" && x.NoticeTypeName == "Important Court Decisions").ToList();
            ViewData["NoticeData"] = allnotice;
            var noticeTypeDetail = detail.GetCourtHirarchyDetail().Where(x => x.LookupName == "Important Court Decisions").ToList();
            ViewData["NoticeType"] = noticeTypeDetail;
            return View();
        }

        public ActionResult DisposedRepresentation(int? noticeId = null, int? categoryId = null)
        {
            var detail = new GeneralDetails();
            var allnotice = detail.GetNoticeDetail(noticeId, categoryId).Where(x => x.EntryTypeName == "Court" && x.NoticeTypeName == "Disposed Representation").ToList();
            ViewData["NoticeData"] = allnotice;
            var noticeTypeDetail = detail.GetCourtHirarchyDetail().Where(x => x.LookupName == "Disposed Representation").ToList();
            ViewData["NoticeType"] = noticeTypeDetail;
            return View();
        }

        private void setUserClaim()
        {
            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
            serializeModel.Id = UserData.UserId;
            serializeModel.FirstName = string.IsNullOrEmpty(UserData.Name) ? string.Empty : UserData.Name;
            serializeModel.Mobile = string.IsNullOrEmpty(UserData.MobileNumber) ? string.Empty : UserData.MobileNumber;
            serializeModel.LastName = string.IsNullOrEmpty(UserData.Username) ? string.Empty : UserData.Username;
            serializeModel.Email = string.IsNullOrEmpty(UserData.Email) ? string.Empty : UserData.Email;

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string userData = serializer.Serialize(serializeModel);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                     1,
                     UserData.Email,
                     DateTime.Now,
                     DateTime.Now.AddMinutes(15),
                     false,
                     userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
        }
        //public ActionResult Register(string actionName)
        //{
        //    if (actionName == "getotpscreen")
        //    {
        //        if (Session["PatientInfo"] != null)
        //        {
        //            ViewData["PatientData"] = Session["PatientInfo"] as PatientInfoModel;
        //        }

        //        ViewData["registerAction"] = "getotpscreen";
        //    }
        //    //return View();
        //    return RedirectToAction("Index");
        //}

        //public ActionResult TempRegister()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult SaveTempPatient(string firstname, string middlename, string lastname, string DOB, string Gender, string mobilenumber, string email, string address, string city, string country, string state, string pincode, string religion, string department, string FatherHusbandName, string MaritalStatus, string title, string aadharNumber)
        //{
        //    string emailRegEx = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        //    if (mobilenumber.Trim().Length != 10)
        //    {
        //        SetAlertMessage("Please Enter correct Mobile Number", "Register");
        //        return RedirectToAction("TempRegister");
        //    }
        //    else if (!Regex.IsMatch(email, emailRegEx, RegexOptions.IgnoreCase))
        //    {
        //        SetAlertMessage("Please Enter correct Email Address", "Register");
        //        return RedirectToAction("TempRegister");
        //    }
        //    else
        //    {
        //        PatientDetails details = new PatientDetails();
        //        PatientDetails _details = new PatientDetails();
        //        int pinResult = 0;
        //        PatientInfoTemporary info = new PatientInfoTemporary();
        //        info.AadharNumber = aadharNumber;
        //        info.FirstName = firstname;
        //        info.MiddleName = middlename;
        //        info.LastName = lastname;
        //        if (!string.IsNullOrEmpty(DOB))
        //            info.DOB = Convert.ToDateTime(DOB);
        //        info.Gender = Gender;
        //        info.MobileNumber = mobilenumber;
        //        info.Email = email;
        //        info.Address = address;
        //        info.Country = country;
        //        info.PinCode = int.TryParse(pincode, out pinResult) ? pinResult : 0;
        //        info.Religion = religion;
        //        info.FatherOrHusbandName = FatherHusbandName;
        //        info.MaritalStatus = MaritalStatus;
        //        //info.Title = Title;
        //        //info.pid = Convert.ToDecimal(pid);
        //        //info.Location = location;

        //        if (!string.IsNullOrEmpty(city))
        //            info.CityId = Convert.ToInt32(city);
        //        else
        //            info.CityId = null;
        //        if (!string.IsNullOrEmpty(state))
        //            info.StateId = Convert.ToInt32(state);
        //        else
        //            info.StateId = null;
        //        if (!string.IsNullOrEmpty(department))
        //            info.DepartmentId = Convert.ToInt32(department);
        //        else
        //            info.DepartmentId = null;
        //        info.RegistrationNumber = VerificationCodeGeneration.GetSerialNumber();
        //        Dictionary<string, object> result;
        //        result = details.SaveTemporaryPatientInfo(info);
        //        if (result["status"].ToString() == CrudStatus.Saved.ToString())
        //        {
        //            var patient = ((PatientInfoTemporary)result["data"]);
        //            SetAlertMessage("Temporary Registration succesfull.Please check Registration No. in your mail!", "Register");
        //            SendMailTemporaryRegistration(info.RegistrationNumber, patient);
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            SetAlertMessage("Temporary Registration failed.", "Register");
        //            return RedirectToAction("TempRegister");
        //        }
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult> GetPatientOTP(string firstname, string middlename, string lastname, string DOB, string Gender, string mobilenumber, string email, string address, string city, string country, string state, string pincode, string religion, string department, string FatherHusbandName, string MaritalStatus, string title, string aadharNumber)
        //{
        //    string emailRegEx = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        //    if (mobilenumber.Trim().Length != 10)
        //    {
        //        SetAlertMessage("Please Enter correct Mobile Number", "Register");
        //        return RedirectToAction("Register");
        //    }
        //    else if (!Regex.IsMatch(email, emailRegEx, RegexOptions.IgnoreCase))
        //    {
        //        SetAlertMessage("Please Enter correct Email Address", "Register");
        //        return RedirectToAction("Register");
        //    }
        //    else
        //    {
        //        PatientDetails details = new PatientDetails();
        //        //var patientInfo = details.GetPatientDetailByMobileNumberANDEmail(mobilenumber.Trim(), email.Trim());
        //        //if (patientInfo != null)
        //        //{
        //        //    SetAlertMessage("Mobile Number or Email Id already in our database, kindly chhange it or reset your account.", "Register");
        //        //    return RedirectToAction("Register");
        //        //}
        //        string verificationCode = VerificationCodeGeneration.GenerateDeviceVerificationCode();
        //        PatientInfoModel pateintModel = getPatientInfoModelForSession(firstname, middlename, lastname, DOB, Gender, mobilenumber, email, address, city, country, pincode, religion, department, verificationCode, state, FatherHusbandName, 0, null, MaritalStatus, title, aadharNumber);
        //        if (pateintModel != null)
        //        {
        //            SendMailFordeviceVerification(firstname, middlename, lastname, email, verificationCode, mobilenumber);
        //            Session["otp"] = verificationCode;
        //            //Session["PatientId"] = ((PatientInfo)result["data"]).PatientId;
        //            Session["PatientInfo"] = pateintModel;
        //            return RedirectToAction("Register", new { actionName = "getotpscreen" });
        //        }
        //        else
        //        {
        //            SetAlertMessage("User is already register", "Register");
        //            return RedirectToAction("Register");
        //        }
        //    }
        //}

        //private async Task SendMailFordeviceVerification(string firstname, string middlename, string lastname, string email, string verificationCode, string mobilenumber)
        //{
        //    await Task.Run(() =>
        //    {
        //        //Send Email
        //        logger.Debug("Send Email Started");
        //        Message msg = new Message()
        //        {
        //            MessageTo = email,
        //            MessageNameTo = firstname + " " + middlename + (string.IsNullOrWhiteSpace(middlename) ? "" : " ") + lastname,
        //            OTP = verificationCode,
        //            Subject = "Verify Mobile Number",
        //            Body = EmailHelper.GetDeviceVerificationEmail(firstname, middlename, lastname, verificationCode)
        //        };
        //        ISendMessageStrategy sendMessageStrategy = new SendMessageStrategyForEmail(msg);
        //        sendMessageStrategy.SendMessages();
        //        logger.Debug("Send Email sucessed");

        //        //Send SMS
        //        logger.Debug("Send SMS started");
        //        msg.Body = "Hello " + string.Format("{0} {1}", firstname, lastname) + "\nAs you requested, here is a OTP " + verificationCode + " you can use it to verify your mobile number before 15 minutes.\n Regards:\n Patient Portal(RMLHIMS)";
        //        msg.MessageTo = mobilenumber;
        //        msg.MessageType = MessageType.OTP;
        //        sendMessageStrategy = new SendMessageStrategyForSMS(msg);
        //        sendMessageStrategy.SendMessages();
        //        logger.Debug("Send SMS sucessed");
        //    });
        //}

        //[HttpPost]
        //public ActionResult verifyOTP(string OTP)
        //{
        //    if (Convert.ToString(Session["otp"]) == OTP)
        //    {
        //        return PaymentTransaction();
        //    }
        //    else
        //    {
        //        SetAlertMessage("OTP not matched", "Register");
        //        return RedirectToAction("Register", new { actionName = "getotpscreen" });
        //    }
        //}
        private void setUserClaim(AdminUser info)
        {
            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
            serializeModel.Id = info.Id;
            serializeModel.FirstName = string.IsNullOrEmpty(info.Name) ? string.Empty : info.Name;
            serializeModel.Mobile = string.IsNullOrEmpty(Convert.ToString(info.MobileNumber)) ? string.Empty : Convert.ToString(info.MobileNumber);
            serializeModel.Email = string.IsNullOrEmpty(info.EmailID) ? string.Empty : info.EmailID;

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string userData = serializer.Serialize(serializeModel);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                     1,
                     info.EmailID,
                     DateTime.Now,
                     DateTime.Now.AddMinutes(15),
                     false,
                     userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
        }
        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult MyProfile(string actionName)
        {
            //if (!string.IsNullOrEmpty(actionName))
            //{
            //    ViewData["Action"] = "Edit";
            //}
            //if (User == null)
            //{
            //    SetAlertMessage("User has been logged out", "Update Profile");
            //    return RedirectToAction("Index");
            //}
            //var patient = GetPatientInfo(User.Id);
            //if (patient != null)
            //{
            //    User.FirstName = patient.FirstName;
            //    User.MiddleName = patient.MiddleName;
            //    User.LastName = patient.LastName;
            //    User.Email = patient.Email;
            //    ViewData["PatientData"] = patient;
            //}
            //else
            //{
            //    SetAlertMessage("User not found", "Update Profile");
            //    return RedirectToAction("Index");
            //}
            return View();
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(string registernumber, string mobilenumber)
        {
            //PatientDetails _detail = new PatientDetails();
            //var patient = _detail.GetPatientDetailByRegistrationNumberAndMobileNumber(registernumber, mobilenumber);
            //if (patient == null)
            //{
            //    SetAlertMessage("Registration or Mobile number is not linked.Please Register or map with CR Number", "Forget Password");
            //    return View();
            //}
            //else
            //{
            //    string resetCode = VerificationCodeGeneration.GetGeneratedResetCode();
            //    //udpate Patient with reset code
            //    patient.ResetCode = resetCode;
            //    _detail.UpdatePatientDetail(patient);
            //    SendMailForgetPassword(registernumber, patient, resetCode);
            //    ViewData["msg"] = "We have Sent you an Email for reset password link.kindly check your email";
            //    return View();
            //}
            return null;
        }

        //private async Task SendMailForgetPassword(string registernumber, PatientInfo patient, string resetCode)
        //{
        //    await Task.Run(() =>
        //    {
        //        string passwordCreateURL = "Home/ResetPassword?resetCode=" + resetCode;
        //        string baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
        //        baseUrl = baseUrl.Replace(":82", "");
        //        Message msg = new Message()
        //        {
        //            MessageTo = patient.Email,
        //            MessageNameTo = patient.FirstName + " " + patient.MiddleName + (string.IsNullOrWhiteSpace(patient.MiddleName) ? "" : " ") + patient.LastName,
        //            Subject = "Forget Password",
        //            Body = EmailHelper.GetForgetPasswordEmail(patient.FirstName, patient.MiddleName, patient.LastName, registernumber, baseUrl + passwordCreateURL)
        //        };

        //        ISendMessageStrategy sendMessageStrategy = new SendMessageStrategyForEmail(msg);
        //        sendMessageStrategy.SendMessages();
        //    });
        //}

        public ActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ChangePassword(string oldpassword, string newpassword, string confirmnewpassword)
        {
            if (newpassword.Trim() != confirmnewpassword.Trim())
            {
                SetAlertMessage("Password and Confirm Password are not match", "password Reset");
                return View();
            }
            else if (newpassword.Trim().Length < 8)
            {
                SetAlertMessage("password must be at least 8 characters long.", "password Create");
                return View();
            }
            else if (!newpassword.Trim().Any(ch => char.IsUpper(ch)))
            {
                SetAlertMessage("password must be at least 1 Upper Case characters.", "password Create");
                return View();
            }
            else if (!newpassword.Trim().Any(ch => char.IsNumber(ch)))
            {
                SetAlertMessage("password must be at least 1 Numeric characters.", "password Create");
                return View();
            }
            else if (!newpassword.Trim().Any(ch => !char.IsLetterOrDigit(ch)))
            {
                SetAlertMessage("password must be at least 1 Special characters.", "password Create");
                return View();
            }
            else
            {
                //PatientDetails _details = new PatientDetails();
                //var result = _details.GetPatientDetailById(User.Id);
                //if (result != null)
                //{
                //    if (result.Password == oldpassword)
                //    {
                //        result.Password = newpassword.Trim();
                //        _details.UpdatePatientDetail(result);
                //        ViewData["msg"] = "Password reset Successfully, please login again.";
                //        return View();
                //    }
                //    else
                //    {
                //        SetAlertMessage("given Password is not correct", "Password Reset");
                //        return View();
                //    }

                //}
                //else
                //{
                //    SetAlertMessage("User Not found", "password Reset");
                //    return View();
                //}
                return null;
            }
        }

        //private void SaveLoginHistory(int patientId)
        //{
        //    string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //    if (string.IsNullOrEmpty(ipAddress))
        //    {
        //        ipAddress = Request.ServerVariables["REMOTE_ADDR"];
        //    }
        //    PatientLoginHistory history = new PatientLoginHistory
        //    {
        //        PatientId = patientId,
        //        LoginDate = DateTime.Now,
        //        IPAddress = ipAddress
        //    };
        //    PatientDetails detail = new PatientDetails();
        //    detail.SavePatientLoginHistory(history);
        //}

        [HttpPost]
        public System.Web.Mvc.JsonResult GetSates()
        {
            //PatientDetails _details = new PatientDetails();
            //return Json(_details.GetStates());
            return null;
        }

        [HttpPost]
        public System.Web.Mvc.JsonResult GetCities(int stateId)
        {
            //PatientDetails _details = new PatientDetails();
            //return Json(_details.GetCities(stateId));
            return null;
        }

        [HttpPost]
        public System.Web.Mvc.JsonResult GetStateByStateId(int stateId)
        {
            //PatientDetails _details = new PatientDetails();
            //return Json(_details.GetStateByStateId(stateId));
            return null;
        }

    }
}
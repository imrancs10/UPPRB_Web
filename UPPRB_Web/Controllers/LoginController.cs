using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using UPPRB_Web.BAL.Login;
using UPPRB_Web.Global;
using UPPRB_Web.Infrastructure.Authentication;
using CaptchaMvc.HtmlHelpers;
using Swashbuckle.Swagger;
using System.Configuration;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using RestSharp;
using System.IO;
using System.Net;
using static System.Net.WebRequestMethods;
using UPPRB_Web.BAL.Masters;

namespace UPPRB_Web.Controllers
{
    public class LoginController : CommonController
    {
        // GET: Login

        public ActionResult Index()
        {
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            //var mobileNumber = 7499242547;
            //var authKey = "009993265aca025f";
            //var message = "Use%20%7B%23otp%23%7D%20as%20your%20OTP%20to%20access%20your%20%7B%23company%23%7D%2C%20OTP%20is%20confidential%20and%20valid%20for%205%20mins%20This%20sms%20sent%20by%20authkey.io";
            ////var message = "Use%201234%20as%20your%20OTP%20to%20access%20your%20Account%2C%20OTP%20is%20confidential%20and%20valid%20for%205%20mins%20This%20sms%20sent%20by%20UPPRPB";
            //var userAuthenticationURI = "https://api.authkey.io/request?authkey=" + authKey + "&mobile=" + mobileNumber + "&country_code=91&sms=" + message + "&sender=8849";
            //if (!string.IsNullOrEmpty(userAuthenticationURI))
            //{
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(userAuthenticationURI);
            //    request.Method = "GET";
            //    request.ContentType = "application/json";
            //    WebResponse response = request.GetResponse();
            //    using (var reader = new StreamReader(response.GetResponseStream()))
            //    {
            //        var ApiStatus = reader.ReadToEnd();
            //        //JsonData data = JsonMapper.ToObject(ApiStatus);
            //        //string status = data["Status"].ToString();
            //        //if (status.ToLower() == "success")
            //        //{
            //        //    postOfficeResult = JsonMapper.ToObject<PostOfficeResult>(ApiStatus);

            //        //}
            //        //if (postOfficeResult != null)
            //        //{
            //        //    grdAreaPostOffice.DataSource = postOfficeResult.PostOffice;
            //        //    grdAreaPostOffice.DataBind();
            //        //}
            //        //else
            //        //{
            //        //    lblMessage.Text = data["Message"].ToString();
            //        //}
            //    }
            //}
            return View();
        }
        [HttpPost]
        public JsonResult SendOTP(string username, string password, string OTP)
        {
            LoginDetails _details = new LoginDetails();
            string _response = string.Empty;
            Enums.LoginMessage message = _details.GetLogin(username, password);
            _response = LoginResponse(message);
            if (message != Enums.LoginMessage.Authenticated)
            {
                return Json("UserNamePasswordInCorrect", JsonRequestBehavior.AllowGet);
            }
            _details.UpdateLoginDetailWithOTP(username, OTP);
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            var mobileNumber = UserData.MobileNumber;
            var authKey = "009993265aca025f";
            var senderId = 8849;
            var userAuthenticationURI = "https://api.authkey.io/request?authkey=" + authKey + "&mobile=" + mobileNumber + "&country_code=91&sid=" + senderId + "&company=account&otp=" + OTP + "";
            if (!string.IsNullOrEmpty(userAuthenticationURI))
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(userAuthenticationURI);
                request.Method = "GET";
                request.ContentType = "application/json";
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var ApiStatus = reader.ReadToEnd();
                }
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetLogin(string username, string password, string otp)
        {
            // Code for validating the CAPTCHA  
            bool isOTPENable = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOTPLogin"]);
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCaptcha"]) == false || this.IsCaptchaValid("Captcha is not valid"))
            {
                LoginDetails _details = new LoginDetails();
                string _response = string.Empty;

                Enums.LoginMessage otpmessage = _details.ValidateOTP(username, otp);
                if (otpmessage == Enums.LoginMessage.Authenticated || isOTPENable == false)
                {
                    Enums.LoginMessage message = _details.GetLogin(username, password);
                    _response = LoginResponse(message);
                    if (message == Enums.LoginMessage.Authenticated)
                    {
                        setUserClaim();
                        _details.InsertLoginDetail();
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(_response, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("OTP is not valid", JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json("Captcha is not valid", JsonRequestBehavior.AllowGet);
            }
        }

        private void setUserClaim()
        {
            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
            serializeModel.Id = UserData.UserId;
            serializeModel.FirstName = string.IsNullOrEmpty(UserData.Name) ? string.Empty : UserData.Name;
            serializeModel.Mobile = string.IsNullOrEmpty(UserData.MobileNumber) ? string.Empty : UserData.MobileNumber;
            serializeModel.LastName = string.IsNullOrEmpty(UserData.Username) ? string.Empty : UserData.Username;
            serializeModel.Email = string.IsNullOrEmpty(UserData.Email) ? string.Empty : UserData.Email;
            serializeModel.RoleId = UserData.RoleId != null ? UserData.RoleId : 0;

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
    }
}
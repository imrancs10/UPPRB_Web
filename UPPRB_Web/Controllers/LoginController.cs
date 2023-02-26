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

namespace UPPRB_Web.Controllers
{
    public class LoginController : CommonController
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ForgetPassword()
        {
            return View();
        }
        public ActionResult GetLogin(string username, string password)
        {
            // Code for validating the CAPTCHA  
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCaptcha"]) == false || this.IsCaptchaValid("Captcha is not valid"))
            {
                LoginDetails _details = new LoginDetails();
                string _response = string.Empty;
                Enums.LoginMessage message = _details.GetLogin(username, password);
                _response = LoginResponse(message);
                if (message == Enums.LoginMessage.Authenticated)
                {
                    setUserClaim();
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    SetAlertMessage(_response, "Login Response");
                    return View("index");
                }
            }
            else
            {
                SetAlertMessage("Captcha is not valid", "Login Response");
                return View("index");
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
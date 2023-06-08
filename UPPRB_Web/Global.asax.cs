using UPPRB_Web.Infrastructure.Authentication;
using UPPRB_Web.Infrastructure.Utility;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using DataLayer;
using System.Linq;
using System.Data.Entity;
using System.Net;
using Org.BouncyCastle.Utilities.Net;
using Antlr.Runtime.Tree;

namespace UPPRB_Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest()
        {
            if (!Request.IsLocal)
            {
                Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"));
            }
            //restrict Click Jacking 
            //HttpContext.Current.Response.AddHeader("x-frame-options", "DENY");
        }
        protected void Application_PreSendRequestHeaders()
        {
            HttpContext.Current.Response.Headers.Add("X-Frame-Options", "DENY");
            HttpContext.Current.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
            HttpContext.Current.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            HttpContext.Current.Response.Headers.Remove("Server");
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalFilters.Filters.Add(new CustomExceptionFilter());
            log4net.Config.XmlConfigurator.Configure();
            //Application["Totaluser"] = 0;
        }
        protected void Session_Start()
        {
            Application.Lock();
            upprbDbEntities _db = new upprbDbEntities();
            var totalUser = _db.Visitor_Detail.Count();
            //GroupBy(x => new { x.Client_IP_Address, x.Device_Type }).Count();
            Application["Totaluser"] = totalUser + 1;
            var ipAddress = GetIPAddress();
            //if (_db.Visitor_Detail.FirstOrDefault(x => x.Client_IP_Address == ipAddress) == null)
            {
                var visitor = new Visitor_Detail()
                {
                    Client_IP_Address = ipAddress,
                    Session_Start_Date = DateTime.UtcNow,
                    Browser_Type = GetBrowserDetails(),
                    Device_Type = GetDeviceType(),
                    Operating_System = GetOperatingSystem()
                };
                _db.Entry(visitor).State = EntityState.Added;
                _db.SaveChanges();
            }
            Application.UnLock();
        }
        public string GetIPAddress()
        {
            //IPHostEntry Host = default(IPHostEntry);
            //string Hostname = null;
            //string ipAddress = string.Empty;
            //Hostname = System.Environment.MachineName;
            //Host = Dns.GetHostEntry(Hostname);
            //foreach (System.Net.IPAddress IP in Host.AddressList)
            //{
            //    if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //    {
            //        ipAddress = Convert.ToString(IP);
            //    }
            //}
            var ipAddress = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).GetValue(1);
            return ipAddress != null ? ipAddress.ToString() : string.Empty;
        }
        public string GetBrowserDetails()
        {
            string browserDetails = string.Empty;
            System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
            return browser.Browser;
        }
        public string GetDeviceType()
        {
            var request = HttpContext.Current.Request;
            if (request.Browser.IsMobileDevice)
                return "Smartphone";
            else
                return "Computer";
        }
        public string GetOperatingSystem()
        {
            HttpBrowserCapabilities browse = Request.Browser;
            string platform = browse.Platform;
            return platform;
        }
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    CustomPrincipalSerializeModel serializeModel = serializer.Deserialize<CustomPrincipalSerializeModel>(authTicket.UserData);

                    CustomPrincipal newUser = new CustomPrincipal(authTicket.Name)
                    {
                        Id = serializeModel.Id,
                        FirstName = serializeModel.FirstName,
                        MiddleName = serializeModel.MiddleName,
                        LastName = serializeModel.LastName,
                        Email = serializeModel.Email,
                        Mobile = serializeModel.Mobile,
                        RoleId = serializeModel.RoleId
                    };

                    HttpContext.Current.User = newUser;
                }
                catch (Exception)
                {
                }

            }
        }

        private void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
    }
}

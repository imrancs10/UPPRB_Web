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

namespace UPPRB_Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest()
        {
            if (!Request.IsLocal)
            {
               // Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"));
            }
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
            Application["Totaluser"] = 0;
        }
        protected void Session_Start()
        {
            Application.Lock();
            Application["Totaluser"] = (int)Application["Totaluser"] + 1;
            Application.UnLock();
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
                        Mobile = serializeModel.Mobile
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

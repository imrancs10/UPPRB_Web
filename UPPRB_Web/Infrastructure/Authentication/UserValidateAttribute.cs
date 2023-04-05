using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using UPPRB_Web.BAL.Login;

namespace UPPRB_Web.Infrastructure.Authentication
{
    [AttributeUsage(AttributeTargets.All)]
    public class UserValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ////write your user right logic
            ////if user has right to do nothig otherwise redirect to error page.
            //string message = "You have no right to view this page.";
            //RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
            //redirectTargetDictionary.Add("area", "");
            //redirectTargetDictionary.Add("action", "Error");
            //redirectTargetDictionary.Add("controller", "Home");
            //redirectTargetDictionary.Add("customMessage", message);
            //filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
            LoginDetails _details = new LoginDetails();
            var islogin = _details.ValidateLoginDetail();
            if (islogin == null || !islogin.Value)
            {
                _details.UpdateLoginDetail();
                FormsAuthentication.SignOut();
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("area", "");
                redirectTargetDictionary.Add("action", "Index");
                redirectTargetDictionary.Add("controller", "Login");
                filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
            }
        }
    }
}
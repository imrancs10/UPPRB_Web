using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPRB_Web.Global
{
    public static class UserData
    {
        public static int UserId
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["userid"] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
                }
                else
                    return 0;
            }
            set { HttpContext.Current.Session["userid"] = value; }
        }
        public static string Username
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["username"] != null)
                {
                    return HttpContext.Current.Session["username"].ToString();
                }
                else
                    return string.Empty;
            }
            set { HttpContext.Current.Session["username"] = value; }
        }
        public static string Name
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["Name"] != null)
                {
                    return HttpContext.Current.Session["Name"].ToString();
                }
                else
                    return string.Empty;
            }
            set { HttpContext.Current.Session["Name"] = value; }
        }
        public static string MobileNumber
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["MobileNumber"] != null)
                {
                    return HttpContext.Current.Session["MobileNumber"].ToString();
                }
                else
                    return string.Empty;
            }
            set { HttpContext.Current.Session["MobileNumber"] = value; }
        }
        public static string Email
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["Email"] != null)
                {
                    return HttpContext.Current.Session["Email"].ToString();
                }
                else
                    return string.Empty;
            }
            set { HttpContext.Current.Session["Email"] = value; }
        }

    }
}
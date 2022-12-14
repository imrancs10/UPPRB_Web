using System.Web;
using System.Web.Mvc;
using UPPRB_Web.Infrastructure.Authentication;

namespace UPPRB_Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomAuthorize());
            //filters.Add(new RequreSecureConnectionFilter());
        }
    }
}

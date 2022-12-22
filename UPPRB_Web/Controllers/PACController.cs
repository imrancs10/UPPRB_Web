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

namespace UPPRB_Web.Controllers
{
    [CustomAuthorize]
    public class PACController : CommonController
    {
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult PACDocument()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllPAC()
        {
            var detail = new GeneralDetails();
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            string filterText = Request["search[value]"];
            var result = detail.GetAllPACDetail();

            if (!string.IsNullOrEmpty(filterText))
            {
                result = result.Where(x => x.AccusedName.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.PS_Name.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.District_Name.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            recordsTotal = result.Count();
            var data = result.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("PACLogin", "Home");
        }
    }
}
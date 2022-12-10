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
            var detail = new GeneralDetails();
            var allnotice = detail.GetNoticeDetail().Where(x => x.EntryTypeName == "PAC").ToList();
            ViewData["NoticeData"] = allnotice;
            return View();
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
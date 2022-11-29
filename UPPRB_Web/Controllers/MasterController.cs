using UPPRB_Web.BAL.Commom;
using UPPRB_Web.Global;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UPPRB_Web.BAL.Masters;
using static iTextSharp.tool.xml.html.HTML;

namespace UPPRB_Web.Controllers
{
    public class MasterController : CommonController
    {
        [HttpPost]
        public JsonResult GetLookupDetail(int? lookupTypeId, string lookupType)
        {
            MasterDetails _details = new MasterDetails();
            if (lookupTypeId == 0)
            {
                lookupTypeId = null;
            }
            return Json(_details.GetLookupDetail(lookupTypeId, lookupType), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetNoticeDetail(int noticeId, int CategoryId)
        {
            var detail = new GeneralDetails();
            var allnotice = detail.GetNoticeDetail(noticeId, CategoryId);
            return Json(allnotice, JsonRequestBehavior.AllowGet);
        }
    }
}
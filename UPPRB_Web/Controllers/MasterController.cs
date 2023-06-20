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
using UPPRB_Web.BAL.Login;

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
        [HttpPost]
        public JsonResult GetStateDetail()
        {
            MasterDetails _details = new MasterDetails();
            var data = _details.GetStateDetail();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPromotionSubject()
        {
            MasterDetails _details = new MasterDetails();
            var data = _details.GetPromotionSubject();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDirectRecruitementSubject()
        {
            MasterDetails _details = new MasterDetails();
            var data = _details.GetDirectRecruitementSubject();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetZoneDetail(int stateId)
        {
            MasterDetails _details = new MasterDetails();
            var data = _details.GetZoneDetail(stateId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetRangeDetail(int zoneId)
        {
            MasterDetails _details = new MasterDetails();
            var data = _details.GetRangeDetail(zoneId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDistrictDetail(int rangeId)
        {
            MasterDetails _details = new MasterDetails();
            var data = _details.GetDistrictDetail(rangeId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPoliceStationDetail(int districtId)
        {
            MasterDetails _details = new MasterDetails();
            var data = _details.GetPoliceStationDetail(districtId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUserPermission()
        {
            int roleId = (int)User.RoleId;
            GeneralDetails _details = new GeneralDetails();
            return Json(_details.GetUserPermission(roleId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUserRole()
        {
            GeneralDetails _details = new GeneralDetails();
            return Json(_details.GetUserRole(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCalenderEvent()
        {
            var detail = new AdminDetails();
            var data = detail.GetEventCalender();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LogoutUser()
        {
            LoginDetails _details = new LoginDetails();
            _details.UpdateLoginDetail();
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPACDetailByPSId(int psId)
        {
            MasterDetails _details = new MasterDetails();
            var data = _details.GetPACDetailByPSId(psId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
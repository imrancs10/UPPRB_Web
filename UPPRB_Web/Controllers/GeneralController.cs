using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UPPRB_Web.Controllers
{
    public class GeneralController : CommonController
    {
        // GET: General
        public ActionResult Information()
        {
            return View();
        }
        public ActionResult AboutRML()
        {
            return View();
        }

        public ActionResult HolidayList()
        {
            return View();
        }
    }
}
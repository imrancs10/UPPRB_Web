using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPRB_Web.Models.Common
{
    public class CommonModel
    {
    }
    public class PACSearchModel
    {
        public int Zone { get; set; }
    }
    public class DayModel
    {
        public int DayId { get; set; }
        public string DayName { get; set; }
    }

    public class  PatientModel
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }

    }
}
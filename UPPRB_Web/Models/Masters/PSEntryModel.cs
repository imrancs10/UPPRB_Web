using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPRB_Web.Models.Masters
{
    public class PSEntryModel
    {
        public int PSId { get; set; }
        public string PSName { get; set; }
        public string DistrictName { get; set; }
        public Nullable<int> DistrictId { get; set; }
    }
}
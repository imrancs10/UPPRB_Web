using UPPRB_Web.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPRB_Web.Models
{
    public class DepartmentOPDModel
    {
        public List<DepartmentModel> Departments { get; set; }
        public PDModel OPDModel { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPRB_Web.Models.Masters
{
    public class PopularRecruitmentModel
    {
        public int Id { get; set; }
        public string RecruitmentName { get; set; }
        public Nullable<int> NoOfSeat { get; set; }
        public Nullable<System.DateTime> RecruitmentStartDate { get; set; }
        public Nullable<System.DateTime> RecruitmentEndDate { get; set; }
        public string fileURL { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<bool> is_active { get; set; }
    }
}
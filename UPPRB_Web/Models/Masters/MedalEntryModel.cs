using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPRB_Web.Models.Masters
{
    public class MedalEntryModel
    {
        public int Id { get; set; }
        public Nullable<int> MedalCategoryId { get; set; }
        public string MedalCategoryName { get; set; }
        public string GivenBy { get; set; }
        public string ToWhom { get; set; }
        public string MedalDescription { get; set; }
        public Nullable<System.DateTime> MedalGivenDate { get; set; }
        public string FileName { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
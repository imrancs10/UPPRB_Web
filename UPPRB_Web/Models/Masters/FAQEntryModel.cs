using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPRB_Web.Models.Masters
{
    public class FAQEntryModel
    {
        public int Id { get; set; }
        public string FAQ_Question { get; set; }
        public string FAQ_Answer { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
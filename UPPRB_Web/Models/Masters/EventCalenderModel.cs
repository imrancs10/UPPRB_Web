using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPRB_Web.Models.Masters
{
    public partial class EventCalenderModel
    {
        public int Id { get; set; }
        public string EventTitle { get; set; }
        public string EventDescription { get; set; }
        public Nullable<System.DateTime> EventDate { get; set; }
        public string FileName { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
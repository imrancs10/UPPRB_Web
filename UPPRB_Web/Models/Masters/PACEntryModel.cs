using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPRB_Web.Models.Masters
{
    public class PACEntryModel
    {
        public int Id { get; set; }
        public Nullable<int> State_Id { get; set; }
        public string State_Name { get; set; }
        public Nullable<int> Zone_Id { get; set; }
        public string Zone_Name { get; set; }
        public Nullable<int> Range_Id { get; set; }
        public string Range_Name { get; set; }
        public Nullable<int> District_Id { get; set; }
        public string District_Name { get; set; }
        public int? PS_Id { get; set; }
        public string PS_Name { get; set; }
        public string ExamineCenterName { get; set; }
        public string Address { get; set; }
        public string FIRNo { get; set; }
        public Nullable<System.DateTime> FIRDate { get; set; }
        public Nullable<System.DateTime> PublishDate { get; set; }
        public string AccusedName { get; set; }
        public string FIRDetails { get; set; }
        public string FileURL { get; set; }
        public string FileUploadName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string PACNumber { get; set; }

    }
}
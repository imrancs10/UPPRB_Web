using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPRB_Web.Models.Masters
{
    public class NoticeModel
    {
        public int Id { get; set; }
        public Nullable<int> NoticeType { get; set; }
        public Nullable<int> NoticeCategoryId { get; set; }
        public Nullable<int> NoticeSubCategoryId { get; set; }
        public string Subject { get; set; }
        [DisplayFormat(DataFormatString = "dd/MM/yyyy}")]

        public Nullable<System.DateTime> NoticeDate { get; set; }
        public string fileURL { get; set; }
        public string filename { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
    }

    public class DoctorModel
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Designation { get; set; }
        public string Degree { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public byte[] Image { get; set; }
    }
    public class DoctorTypeModel
    {
        public int Id { get; set; }
        public string DoctorType { get; set; }
    }
        public class MasterLookupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
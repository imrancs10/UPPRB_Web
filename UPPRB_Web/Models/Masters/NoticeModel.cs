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
        public Nullable<int> EntryTypeId { get; set; }
        public string Subject { get; set; }
        [DisplayFormat(DataFormatString = "dd/MM/yyyy}")]

        public Nullable<System.DateTime> NoticeDate { get; set; }
        public string fileURL { get; set; }
        public string filename { get; set; }
        public bool? IsNew { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string NoticeTypeName { get; set; }
        public string NoticeCategoryName { get; set; }
        public string EntryTypeName { get; set; }
    }

    public class NoticeTypeModel
    {
        public NoticeTypeModel()
        {
            NoticeCategories = new List<NoticeCategoryModel>();
        }
        public int LookupId { get; set; }
        public string LookupName { get; set; }
        public List<NoticeCategoryModel> NoticeCategories { get; set; }
    }
    public class NoticeCategoryModel
    {
        public int LookupId { get; set; }
        public string LookupName { get; set; }
    }
    public class MasterLookupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Notice
    {
        public int Id { get; set; }
        public Nullable<int> NoticeType { get; set; }
        public Nullable<int> NoticeCategoryId { get; set; }
        public Nullable<int> EntryTypeId { get; set; }
        public string Subject { get; set; }
        public Nullable<System.DateTime> NoticeDate { get; set; }
        public string fileURL { get; set; }
        public string filename { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<bool> IsNew { get; set; }
    }
}

﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class upprbDbEntities : DbContext
    {
        public upprbDbEntities()
            : base("name=upprbDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Lookup> Lookups { get; set; }
        public virtual DbSet<PACUser> PACUsers { get; set; }
        public virtual DbSet<Enquiry> Enquiries { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<DistrictMaster> DistrictMasters { get; set; }
        public virtual DbSet<PSMaster> PSMasters { get; set; }
        public virtual DbSet<RangeMaster> RangeMasters { get; set; }
        public virtual DbSet<StateMaster> StateMasters { get; set; }
        public virtual DbSet<ZoneMaster> ZoneMasters { get; set; }
        public virtual DbSet<Notice> Notices { get; set; }
        public virtual DbSet<PACEntry> PACEntries { get; set; }
        public virtual DbSet<DirectRecruitementDetail> DirectRecruitementDetails { get; set; }
        public virtual DbSet<PromotionDetail> PromotionDetails { get; set; }
        public virtual DbSet<FAQDetail> FAQDetails { get; set; }
        public virtual DbSet<MedalDetail> MedalDetails { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<EventCalender> EventCalenders { get; set; }
        public virtual DbSet<Visitor_Detail> Visitor_Detail { get; set; }
        public virtual DbSet<LoginDetail> LoginDetails { get; set; }
        public virtual DbSet<AdminUser> AdminUsers { get; set; }
    }
}

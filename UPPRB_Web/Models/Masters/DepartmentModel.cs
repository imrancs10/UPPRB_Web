using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPRB_Web.Models.Masters
{
    public class DepartmentModel
    {
        public int DepartmentId { get; set; }
        public string DeparmentName { get; set; }
        public string DepartmentUrl { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public byte[] Image { get; set; }
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
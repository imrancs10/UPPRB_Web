using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPRB_Web.Models.Masters
{
    public class AdminUserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Name { get; set; }
        public string EmailID { get; set; }
        public Nullable<long> MobileNumber { get; set; }
        public Nullable<int> RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
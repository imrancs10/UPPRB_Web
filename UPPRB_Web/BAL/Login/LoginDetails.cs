using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using UPPRB_Web.Global;
using System.Data.Entity;

namespace UPPRB_Web.BAL.Login
{
    public class LoginDetails
    {
        upprbDbEntities _db = null;

        /// <summary>
        /// Get Authenticate User credentials
        /// </summary>
        /// <param name="UserName">Username</param>
        /// <param name="Password">Password</param>
        /// <returns>Enums</returns>
        public Enums.LoginMessage GetLogin(string UserName, string Password)
        {
            string _passwordHash = Utility.GetHashString(Password);
            _db = new upprbDbEntities();

            var _userLogin = _db.AdminUsers.Where(x => x.UserName.Equals(UserName) && x.Password.Equals(Password) && x.IsActive == true).FirstOrDefault();

            if (_userLogin != null)
            {
                if (_userLogin != null)
                {
                    if (_userLogin.IsActive == false)
                        return Enums.LoginMessage.UserBlocked;
                }
                UserData.UserId = _userLogin.Id;
                UserData.Username = _userLogin.UserName;
                UserData.Name = _userLogin.Name;
                UserData.MobileNumber = Convert.ToString(_userLogin.MobileNumber);
                UserData.Email = _userLogin.EmailID;
                UserData.RoleId = _userLogin.RoleId;
                return Enums.LoginMessage.Authenticated;
            }
            else
                return Enums.LoginMessage.InvalidCreadential;
        }
        public Enums.LoginMessage PACLogin(string UserName, string Password)
        {
            _db = new upprbDbEntities();

            var _userLogin = _db.PACUsers.Where(x => x.UserName.Equals(UserName) && x.Password.Equals(Password) && x.IsActive == true).FirstOrDefault();

            if (_userLogin != null)
            {
                if (_userLogin != null)
                {
                    if (_userLogin.IsActive == false)
                        return Enums.LoginMessage.UserBlocked;
                }
                UserData.UserId = _userLogin.Id;
                UserData.Username = _userLogin.UserName;
                UserData.Name = _userLogin.Name;
                UserData.MobileNumber = Convert.ToString(_userLogin.MobileNumber);
                UserData.Email = _userLogin.EmailID;
                return Enums.LoginMessage.Authenticated;
            }
            else
                return Enums.LoginMessage.InvalidCreadential;
        }
    }
}
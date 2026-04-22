using DataLayer;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;
using UPPRB_Web.Global;

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

            if (_userLogin!=null && _userLogin.userLockedDateTime != null && DateTime.Now < _userLogin.userLockedDateTime.Value.AddMinutes(30))
            {
                return Enums.LoginMessage.UserLocked;
            }
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

                //update failed login detail
                _userLogin.loginfailed = 0;
                _userLogin.userLockedDateTime = null;
                _db.Entry(_userLogin).State = EntityState.Modified;
                _db.SaveChanges();

                return Enums.LoginMessage.Authenticated;
            }
            else
                return Enums.LoginMessage.InvalidCreadential;
        }

        public Enums.LoginMessage updateLoginFail(string UserName)
        {
            _db = new upprbDbEntities();

            var _userLogin = _db.AdminUsers.Where(x => x.UserName.Equals(UserName) && x.IsActive == true).FirstOrDefault();

            if (_userLogin != null)
            {
                if (_userLogin.loginfailed == 3)
                {
                    if (DateTime.Now < _userLogin.userLockedDateTime.Value.AddMinutes(30))
                    {
                        return Enums.LoginMessage.UserLocked;
                    }
                    else
                    {
                        //update failed login detail
                        _userLogin.loginfailed = 0;
                        _userLogin.userLockedDateTime = null;
                        _db.Entry(_userLogin).State = EntityState.Modified;
                        _db.SaveChanges();
                    }

                }
                else
                {
                    _userLogin.loginfailed = _userLogin.loginfailed != null ? _userLogin.loginfailed + 1 : 1;
                    if (_userLogin.loginfailed == 3)
                    {
                        _userLogin.userLockedDateTime = DateTime.Now;
                    }
                    _db.Entry(_userLogin).State = EntityState.Modified;
                    _db.SaveChanges();
                    return Enums.LoginMessage.InvalidCreadential;
                }

            }
            return Enums.LoginMessage.InvalidCreadential;
        }

        public int getRemainingLockedTime(string UserName)
        {
            _db = new upprbDbEntities();

            var _userLogin = _db.AdminUsers.Where(x => x.UserName.Equals(UserName) && x.IsActive == true).FirstOrDefault();

            if (_userLogin != null)
            {
                return (_userLogin.userLockedDateTime.Value.Minute+30)-DateTime.Now.Minute;
            }
            return 0;
        }

                public Enums.LoginMessage ValidateOTP(string UserName, string OTP)
        {
            _db = new upprbDbEntities();
            var _userLogin = _db.AdminUsers.Where(x => x.UserName.Equals(UserName) && x.otp_number == OTP).FirstOrDefault();

            if (_userLogin != null)
            {
                return Enums.LoginMessage.Authenticated;
            }
            else
                return Enums.LoginMessage.InvalidCreadential;
        }
        public Enums.LoginMessage ValidatePACLoginOTP(string UserName, string OTP)
        {
            _db = new upprbDbEntities();
            var _userLogin = _db.PACUsers.Where(x => x.UserName.Equals(UserName) && x.otp_number == OTP).FirstOrDefault();

            if (_userLogin != null)
            {
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

        public bool InsertLoginDetail()
        {
            _db = new upprbDbEntities();
            var login = new LoginDetail()
            {
                IsLogin = true,
                LoginAt = DateTime.UtcNow,
                UserId = UserData.UserId
            };
            _db.Entry(login).State = EntityState.Added;
            _db.SaveChanges();
            return true;
        }
        public bool UpdateLoginDetail()
        {
            _db = new upprbDbEntities();
            var _userLogin = _db.LoginDetails.Where(x => x.UserId == UserData.UserId && x.IsLogin == true).ToList();
            foreach (var login in _userLogin)
            {
                login.IsLogin = false;
                login.LogoutAt = DateTime.UtcNow;
            }
            _db.SaveChanges();
            return true;
        }
        public bool UpdateLoginDetailWithOTP(string username, string otpNumber)
        {
            _db = new upprbDbEntities();
            var _userLogin = _db.AdminUsers.Where(x => x.UserName == username).FirstOrDefault();
            _userLogin.otp_number = otpNumber;
            _db.SaveChanges();
            return true;
        }
        public bool UpdatePACLoginDetailWithOTP(string username, string otpNumber)
        {
            _db = new upprbDbEntities();
            var _userLogin = _db.PACUsers.Where(x => x.UserName == username).FirstOrDefault();
            _userLogin.otp_number = otpNumber;
            _db.SaveChanges();
            return true;
        }
        public bool? ValidateLoginDetail()
        {
            _db = new upprbDbEntities();
            var _userLogin = _db.LoginDetails.Where(x => x.UserId == UserData.UserId).OrderByDescending(x => x.LoginAt).FirstOrDefault();
            return _userLogin?.IsLogin;
        }
    }
}
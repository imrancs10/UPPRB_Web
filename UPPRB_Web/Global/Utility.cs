using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace UPPRB_Web.Global
{
    public static class Utility
    {
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source?.IndexOf(value, comparisonType) >= 0;
        }
        private static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

        public static string GetAppSettingKey(string key)
        {
            try
            {
                string result = ConfigurationManager.AppSettings.Get(key);
                return string.IsNullOrEmpty(result) ? string.Empty : result;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }
    }

    public static class WebSession
    {

        public static int PatientId
        {
            get { return HttpContext.Current.Session["PatientId"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["PatientId"]); }
            set { HttpContext.Current.Session["PatientId"] = value; }
        }
        public static int AppointmentSlot
        {
            get { return HttpContext.Current.Session["AppointmentSlot"] == null ?Convert.ToInt32(Utility.GetAppSettingKey("AppointmentPeriodInMinuts")) : Convert.ToInt32(HttpContext.Current.Session["AppointmentSlot"]); }
            set { HttpContext.Current.Session["AppointmentSlot"] = value; }
        }
        public static int CalenderPeriod
        {
            get { return HttpContext.Current.Session["CalenderPeriod"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["CalenderPeriod"]); }
            set { HttpContext.Current.Session["CalenderPeriod"] = value; }
        }
        public static string AppointmentMessage
        {
            get { return HttpContext.Current.Session["AppointmentMessage"] == null ? string.Empty : HttpContext.Current.Session["AppointmentMessage"].ToString(); }
            set { HttpContext.Current.Session["AppointmentMessage"] = value; }
        }
        public static bool IsActiveAppointmentMessage
        {
            get { return HttpContext.Current.Session["IsActiveAppointmentMessage"] == null ? false :Convert.ToBoolean(HttpContext.Current.Session["IsActiveAppointmentMessage"].ToString()); }
            set { HttpContext.Current.Session["IsActiveAppointmentMessage"] = value; }
        }
        public static int AppointmentLimitPerUser
        {
            get { return HttpContext.Current.Session["AppointmentLimitPerUser"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["AppointmentLimitPerUser"]); }
            set { HttpContext.Current.Session["AppointmentLimitPerUser"] = value; }
        }
        public static int AppointmentCancelPeriod
        {
            get { return HttpContext.Current.Session["ApoointmentCancelPeriod"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["ApoointmentCancelPeriod"]); }
            set { HttpContext.Current.Session["ApoointmentCancelPeriod"] = value; }
        }
        public static string AutoCancelMessage
        {
            get { return HttpContext.Current.Session["AutoCancelMessage"] == null ? string.Empty : HttpContext.Current.Session["AutoCancelMessage"].ToString(); }
            set { HttpContext.Current.Session["AutoCancelMessage"] = value; }
        }

        public static string PatientRegNo
        {
            get { return HttpContext.Current.Session["PatientRegNo"] == null ? string.Empty : HttpContext.Current.Session["PatientRegNo"].ToString(); }
            set { HttpContext.Current.Session["PatientRegNo"] = value; }
        }
        public static string PatientCRNo
        {
            get { return HttpContext.Current.Session["PatientCRNo"] == null ? string.Empty : HttpContext.Current.Session["PatientCRNo"].ToString(); }
            set { HttpContext.Current.Session["PatientCRNo"] = value; }
        }
        public static int PatientLedgerPeriodInMonth
        {
            get { return HttpContext.Current.Session["PatientLedgerPeriodInMonth"] == null ? Convert.ToInt32(Utility.GetAppSettingKey("PatientLedgerPeriodInMonth")) : Convert.ToInt32(HttpContext.Current.Session["PatientLedgerPeriodInMonth"].ToString()); }
        }

        public static string HospitalLogo
        {
            get { return HttpContext.Current.Session["HospitalLogo"] == null ? string.Empty : HttpContext.Current.Session["HospitalLogo"].ToString(); }
            set { HttpContext.Current.Session["HospitalLogo"] = value; }
        }

        public static string PatientMobile
        {
            get { return HttpContext.Current.Session["PatientMobile"] == null ? string.Empty : HttpContext.Current.Session["PatientMobile"].ToString(); }
            set { HttpContext.Current.Session["PatientMobile"] = value; }
        }
        public static string PatientAddress
        {
            get { return HttpContext.Current.Session["PatientAddress"] == null ? string.Empty : HttpContext.Current.Session["PatientAddress"].ToString(); }
            set { HttpContext.Current.Session["PatientAddress"] = value; }
        }
        public static string PatientCity
        {
            get { return HttpContext.Current.Session["PatientCity"] == null ? string.Empty : HttpContext.Current.Session["PatientCity"].ToString(); }
            set { HttpContext.Current.Session["PatientCity"] = value; }
        }

        public static string PatientGender
        {
            get { return HttpContext.Current.Session["PatientGender"] == null ? string.Empty : HttpContext.Current.Session["PatientGender"].ToString(); }
            set { HttpContext.Current.Session["PatientGender"] = value; }
        }

        public static string PatientName
        {
            get { return HttpContext.Current.Session["PatientName"] == null ? string.Empty : HttpContext.Current.Session["PatientName"].ToString(); }
            set { HttpContext.Current.Session["PatientName"] = value; }
        }

        public static string PatientDOB
        {
            get { return HttpContext.Current.Session["PatientDOB"] == null ? string.Empty : HttpContext.Current.Session["PatientDOB"].ToString(); }
            set { HttpContext.Current.Session["PatientDOB"] = value; }
        }
        public static int PatientAge
        {
            get { return HttpContext.Current.Session["PatientAge"] == null ? default(int):Convert.ToInt32(HttpContext.Current.Session["PatientAge"].ToString()); }
            set { HttpContext.Current.Session["PatientAge"] = value; }
        }
        public static int? DepartmentId
        {
            get { return HttpContext.Current.Session["DepartmentId"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["DepartmentId"]); }
            set { HttpContext.Current.Session["DepartmentId"] = value; }
        }
        public static int? DoctorId
        {
            get { return HttpContext.Current.Session["DoctorId"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["DoctorId"]); }
            set { HttpContext.Current.Session["DoctorId"] = value; }
        }
    }
}
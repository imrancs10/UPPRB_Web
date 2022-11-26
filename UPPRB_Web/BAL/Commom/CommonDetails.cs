using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using UPPRB_Web.Models.Common;
using UPPRB_Web.Global;
using System.IO;

namespace UPPRB_Web.BAL.Commom
{
    public class CommonDetails
    {
        upprbDbEntities _db = null;
        private string[] Months = new string[] { "January", "Febuary", "March", "April", "May", "June", "July", "August", "Sepetember", "October", "November", "December" };

        public List<DayModel> DaysList()
        {
            //_db = new upprbDbEntities();
            //var _list = (from day in _db.DayMasters
            //             select new DayModel
            //             {
            //                 DayId = day.DayId,
            //                 DayName = day.DayName
            //             }).ToList();
            //return _list != null ? _list : new List<DayModel>();
            return null;
        }

        public List<PatientModel> PatientSearch(string _searchValue)
        {
            //_db = new upprbDbEntities();
            //return _db.PatientInfoes.Where(x => x.CRNumber.Contains(_searchValue) || x.RegistrationNumber.Contains(_searchValue) || x.MobileNumber.Contains(_searchValue) || x.LastName.Contains(_searchValue) || x.FirstName.Contains(_searchValue) || x.Email.Contains(_searchValue))
            //                        .Select(x => new PatientModel { PatientId = x.PatientId, PatientName = x.FirstName + " " + x.LastName }).ToList();
            return null;
        }

        public string ReportFileUpload(HttpPostedFileBase file, Enums.ReportType _type, string RefNo)
        {
            try
            {
                string baseUrl = AppDomain.CurrentDomain.BaseDirectory.ToString();
                string baseUrlServer = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                                        HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
                string filepath = baseUrl;
                string serverfilepath = baseUrlServer;
                if (file.ContentLength > 0)
                {
                    filepath += "\\Reports";
                    serverfilepath += "//Reports";
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                        Directory.CreateDirectory(filepath + "\\Bill");
                        Directory.CreateDirectory(filepath + "\\Lab");
                    }
                    else
                    {
                        if (_type == Enums.ReportType.Bill)
                        {
                            filepath += "\\Bill";
                            serverfilepath += "//Bill";
                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }
                            filepath += "\\" + DateTime.Now.Year.ToString();
                            serverfilepath += "//" + DateTime.Now.Year.ToString();
                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }
                            filepath += "\\" + Months[DateTime.Now.Month];
                            serverfilepath += "//" + Months[DateTime.Now.Month];
                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }
                        }
                        else if (_type == Enums.ReportType.Lab)
                        {
                            filepath += "\\Lab";
                            serverfilepath += "//Lab";
                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }
                            filepath += "\\" + DateTime.Now.Year.ToString();
                            serverfilepath += "//" + DateTime.Now.Year.ToString();
                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }
                            filepath += "\\" + Months[DateTime.Now.Month];
                            serverfilepath += "//" + Months[DateTime.Now.Month];
                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }
                        }
                        filepath += "\\" + RefNo;
                        serverfilepath += "//" + RefNo;
                        if (file.ContentLength > 0)
                        {
                            filepath += Path.GetExtension(file.FileName);
                            serverfilepath += Path.GetExtension(file.FileName);
                            file.SaveAs(filepath);
                            return serverfilepath;
                        }
                    }
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
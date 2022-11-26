using DataLayer;
using UPPRB_Web.Global;
using UPPRB_Web.Models.Patient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using static UPPRB_Web.Global.Enums;

namespace UPPRB_Web.BAL.Patient
{
    public class PatientDetails
    {
        upprbDbEntities _db = null;
        public Dictionary<string, object> GetPatientDetail(string UserId, string Password)
        {
            _db = new upprbDbEntities();
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            //string hashPassword = Utility.GetHashString(Password);
            var result = _db.PatientInfoes.Include(x => x.Department).Include(x => x.City)
                                    .Include(x => x.PatientLoginEntries)
                                    .Where(x => (x.RegistrationNumber == UserId || x.CRNumber == UserId)
                                         && x.Password == Password
                                         && DbFunctions.TruncateTime(x.ValidUpto) >= DbFunctions.TruncateTime(DateTime.Now))
                                    .FirstOrDefault();
            if (result != null)
            {
                resultDic.Add("status", CrudStatus.Updated);
                resultDic.Add("data", result);

                WebSession.PatientRegNo = result.RegistrationNumber;
                WebSession.PatientCRNo = result.CRNumber;
                WebSession.PatientId = result.PatientId;
                WebSession.PatientDOB = result.DOB == null ? default(DateTime).ToShortDateString() : Convert.ToDateTime(result.DOB).ToShortDateString();
                WebSession.PatientGender = result.Gender;
                WebSession.PatientMobile = result.MobileNumber;
                WebSession.PatientAddress = result.Address;
                WebSession.PatientCity = result.City.CityName;
                WebSession.PatientName = string.Format("{0} {1}", result.FirstName, result.LastName);
                DateTime zeroTime = new DateTime(1, 1, 1);

                DateTime a = Convert.ToDateTime(result.DOB);
                DateTime b = DateTime.Now;

                TimeSpan span = b - a;
                // Because we start at year 1 for the Gregorian
                // calendar, we must subtract a year here.
                int years = (zeroTime + span).Year - 1;
                WebSession.PatientAge = years;

                var loginEntry = (from obj in result.PatientLoginEntries.AsEnumerable()
                                  where obj.Locked == true
                                    && obj.LoginAttemptDate.Value.Date == DateTime.Now.Date
                                    && obj.PatientId == result.PatientId
                                  select obj);
                var appSetting = _db.AppointmentSettings.Where(x => x.IsActive).FirstOrDefault();
                if (appSetting != null)
                {
                    WebSession.AppointmentCancelPeriod = appSetting.AppointmentCancelPeriod;
                    WebSession.AppointmentLimitPerUser = appSetting.AppointmentLimitPerUser;
                    WebSession.AppointmentMessage = appSetting.AppointmentMessage;
                    WebSession.AppointmentSlot = appSetting.AppointmentSlot;
                    WebSession.CalenderPeriod = appSetting.CalenderPeriod;
                    WebSession.AutoCancelMessage = appSetting.AutoCancelMessage;
                    WebSession.IsActiveAppointmentMessage = appSetting.IsActiveAppointmentMessage;
                }

                if (loginEntry.Count() == 0)
                {
                    //Reset Failed login history
                    var _loginRow = _db.PatientLoginEntries.Where(x => x.PatientId == result.PatientId)
                                                  .FirstOrDefault();
                    if (_loginRow != null)
                    {
                        _db.PatientLoginEntries.Remove(_loginRow);
                        _db.SaveChanges();
                    }
                }
            }
            else
            {
                var data = _db.PatientInfoes.Include(x => x.Department)
                                    .Include(x => x.PatientLoginEntries)
                                    .Where(x => x.RegistrationNumber == UserId
                                         && x.Password == Password)
                                    .FirstOrDefault();
                if (data != null)
                {
                    resultDic.Add("status", CrudStatus.RegistrationExpired);
                    resultDic.Add("data", data);
                }
                else
                {
                    resultDic.Add("status", CrudStatus.DataNotFound);
                    resultDic.Add("data", data);
                }

            }
            return resultDic;
        }

        public PatientInfo GetPatientDetailByRegistrationNumber(string UserId)
        {
            _db = new upprbDbEntities();
            return _db.PatientInfoes.Where(x => x.RegistrationNumber == UserId).FirstOrDefault();
        }

        public PatientInfo GetPatientDetailByRegistrationNumberAndMobileNumber(string regNumber, string mobilenumber)
        {
            _db = new upprbDbEntities();
            return _db.PatientInfoes.Where(x => x.RegistrationNumber == regNumber || x.CRNumber == regNumber && x.MobileNumber == mobilenumber).FirstOrDefault();
        }

        public PatientInfo GetPatientDetailByRegistrationNumberOrCRNumber(string UserId)
        {
            _db = new upprbDbEntities();
            return _db.PatientInfoes.Where(x => x.RegistrationNumber == UserId || x.CRNumber == UserId).FirstOrDefault();
        }

        public PatientInfo GetPatientDetailByresetCode(string resetCode)
        {
            _db = new upprbDbEntities();
            return _db.PatientInfoes.Where(x => x.ResetCode == resetCode).FirstOrDefault();
        }

        public PatientInfo GetPatientDetailByMobileNumberOrEmail(string UserId)
        {
            _db = new upprbDbEntities();
            return _db.PatientInfoes.Where(x => x.MobileNumber == UserId.Trim() || x.Email == UserId.Trim()).FirstOrDefault();
        }
        public PatientInfo GetPatientDetailByMobileNumberANDEmail(string mobileNo, string emailId)
        {
            _db = new upprbDbEntities();
            return _db.PatientInfoes.Where(x => (!string.IsNullOrEmpty(x.MobileNumber) && x.MobileNumber.Equals(mobileNo)) || (!string.IsNullOrEmpty(x.Email) && x.Email.Equals(emailId))).FirstOrDefault();
        }


        public PatientInfo GetPatientDetailById(int Id)
        {
            _db = new upprbDbEntities();

            return _db.PatientInfoes.Include(x => x.Department).Include(x => x.PatientTransactions).Include(x => x.AppointmentInfoes).Where(x => x.PatientId.Equals(Id)).FirstOrDefault();
        }

        public PatientInfo UpdatePatientDetail(PatientInfo info)
        {
            _db = new upprbDbEntities();
            var _patientRow = _db.PatientInfoes.Where(x => x.PatientId.Equals(info.PatientId)).FirstOrDefault();
            if (_patientRow != null)
            {
                _patientRow.OTP = info.OTP;
                _patientRow.ResetCode = info.ResetCode;
                _patientRow.ValidUpto = info.ValidUpto > DateTime.Now ? info.ValidUpto : _patientRow.ValidUpto;
                _patientRow.Password = !string.IsNullOrEmpty(info.Password) ? info.Password : _patientRow.Password;
                _patientRow.CRNumber = !string.IsNullOrEmpty(info.CRNumber) ? info.CRNumber : _patientRow.CRNumber;
                _patientRow.pid = decimal.TryParse(Convert.ToString(info.pid), out decimal externalPid) ? externalPid : _patientRow.pid;
                _patientRow.Location = !string.IsNullOrEmpty(info.Location) ? info.Location : _patientRow.Location;
                _patientRow.RegistrationNumber = !string.IsNullOrEmpty(info.RegistrationNumber) ? info.RegistrationNumber : _patientRow.RegistrationNumber;
                _db.Entry(_patientRow).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return _patientRow;
        }

        public PatientInfoCRClone UpdatePatientDetailClone(PatientInfoCRClone info)
        {
            _db = new upprbDbEntities();
            var _patientRow = _db.PatientInfoCRClones.Where(x => x.PatientId.Equals(info.PatientId)).FirstOrDefault();
            if (_patientRow != null)
            {
                _patientRow.OTP = info.OTP;
                _patientRow.ResetCode = info.ResetCode;
                _patientRow.ValidUpto = info.ValidUpto > DateTime.Now ? info.ValidUpto : _patientRow.ValidUpto;
                _patientRow.Password = !string.IsNullOrEmpty(info.Password) ? info.Password : _patientRow.Password;
                _patientRow.CRNumber = !string.IsNullOrEmpty(info.CRNumber) ? info.CRNumber : _patientRow.CRNumber;
                _patientRow.RegistrationNumber = !string.IsNullOrEmpty(info.RegistrationNumber) ? info.RegistrationNumber : _patientRow.RegistrationNumber;
                _db.Entry(_patientRow).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return _patientRow;
        }

        public PatientInfo UpdatePatientValidity(PatientInfo info)
        {
            _db = new upprbDbEntities();
            var _patientRow = _db.PatientInfoes.Include(x => x.City).Include(x => x.State).Include(x => x.PatientTransactions).Where(x => x.PatientId.Equals(info.PatientId)).FirstOrDefault();
            if (_patientRow != null)
            {
                if (_patientRow.ValidUpto.Value.Date > DateTime.Now.Date)
                    _patientRow.ValidUpto = _patientRow.ValidUpto.Value.AddMonths(Convert.ToInt32(ConfigurationManager.AppSettings["RegistrationValidityInMonth"]));
                else
                    _patientRow.ValidUpto = DateTime.Now.AddMonths(Convert.ToInt32(ConfigurationManager.AppSettings["RegistrationValidityInMonth"]));
                _db.Entry(_patientRow).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return _patientRow;
        }

        public PatientInfo UpdatePatientHISSyncStatus(PatientInfo info)
        {
            _db = new upprbDbEntities();
            var _patientRow = _db.PatientInfoes.Where(x => x.PatientId.Equals(info.PatientId)).FirstOrDefault();
            if (_patientRow != null)
            {
                _patientRow.RenewalStatusHIS = !string.IsNullOrEmpty(info.RenewalStatusHIS) ? info.RenewalStatusHIS : _patientRow.RenewalStatusHIS;
                _patientRow.RegistrationStatusHIS = !string.IsNullOrEmpty(info.RegistrationStatusHIS) ? info.RegistrationStatusHIS : _patientRow.RegistrationStatusHIS;
                _db.Entry(_patientRow).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return _patientRow;
        }

        public bool VerifyPatientOTP(int patientId, string OTP)
        {
            _db = new upprbDbEntities();
            var result = _db.PatientInfoes.Where(x => x.PatientId.Equals(patientId) && x.OTP == OTP).FirstOrDefault();
            if (result != null)
            {
                return true;
            }
            else
                return false;
        }

        public Dictionary<string, object> CreateOrUpdatePatientDetail(PatientInfo info)
        {
            _db = new upprbDbEntities();
            Dictionary<string, object> result = new Dictionary<string, object>();
            int _effectRow = 0;
            if (info.PatientId > 0)
            {
                var _patientRow = _db.PatientInfoes.Include(x => x.Department).Where(x => x.PatientId.Equals(info.PatientId)).FirstOrDefault();
                if (_patientRow != null)
                {
                    _patientRow.Email = info.Email;
                    _patientRow.Photo = info.Photo != null ? info.Photo : _patientRow.Photo;
                    _db.Entry(_patientRow).State = EntityState.Modified;
                    _db.SaveChanges();
                    result.Add("status", CrudStatus.Saved.ToString());
                    result.Add("data", _patientRow);
                    return result;
                }
                else
                {
                    result.Add("status", CrudStatus.NotSaved.ToString());
                    result.Add("data", info);
                    return result;
                }
            }
            else
            {
                info.ValidUpto = DateTime.Now.AddMonths(Convert.ToInt32(ConfigurationManager.AppSettings["RegistrationValidityInMonth"]));
                info.CreatedDate = DateTime.Now;
                _db.Entry(info).State = EntityState.Added;
                _effectRow = _db.SaveChanges();
                result.Add("status", CrudStatus.Saved.ToString());
                info.Department = _db.PatientInfoes.Include(x => x.Department).FirstOrDefault().Department;
                result.Add("data", info);
                return result;
            }
        }

        public Dictionary<string, object> SaveTemporaryPatientInfo(PatientInfoTemporary info)
        {
            _db = new upprbDbEntities();
            Dictionary<string, object> result = new Dictionary<string, object>();
            int _effectRow = 0;
            info.ValidUpto = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["TemporaryRegistrationValidityInDay"]));
            info.CreatedDate = DateTime.Now;
            _db.Entry(info).State = EntityState.Added;
            _effectRow = _db.SaveChanges();
            result.Add("status", CrudStatus.Saved.ToString());
            info.Department = _db.PatientInfoes.Include(x => x.Department).FirstOrDefault().Department;
            result.Add("data", info);
            return result;
        }

        public Dictionary<string, object> CreateOrUpdatePatientDetailClone(PatientInfoCRClone info)
        {
            _db = new upprbDbEntities();
            Dictionary<string, object> result = new Dictionary<string, object>();
            int _effectRow = 0;
            bool foundEmail = false;
            var _deptRow = _db.PatientInfoCRClones.Where(x => (x.Email != null && x.Email != "" && x.Email.Equals(info.Email))).FirstOrDefault();
            if (_deptRow != null)
            {
                foundEmail = true;
            }
            info.ValidUpto = DateTime.Now.AddMonths(Convert.ToInt32(ConfigurationManager.AppSettings["RegistrationValidityInMonth"]));
            info.CreatedDate = DateTime.Now;
            _db.Entry(info).State = EntityState.Added;
            _effectRow = _db.SaveChanges();
            result.Add("status", CrudStatus.Saved.ToString());
            info.Department = _db.PatientInfoCRClones.Include(x => x.Department).FirstOrDefault().Department;
            result.Add("data", info);
            if (foundEmail == true)
            {
                result.Add("status", CrudStatus.DataAlreadyExist.ToString());
            }
            return result;
        }

        public Dictionary<string, object> SavePatientTransaction(PatientTransaction info)
        {
            _db = new upprbDbEntities();
            Dictionary<string, object> result = new Dictionary<string, object>();
            int _effectRow = 0;
            _db.Entry(info).State = EntityState.Added;
            _effectRow = _db.SaveChanges();
            result.Add("status", CrudStatus.Saved.ToString());
            result.Add("data", info);
            return result;
        }

        public bool SavePatientLoginHistory(PatientLoginHistory info)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            _db.Entry(info).State = EntityState.Added;
            _effectRow = _db.SaveChanges();
            return _effectRow > 0;
        }

        public PatientLoginEntry SavePatientLoginFailedHistory(PatientLoginEntry info)
        {
            int _effectRow = 0;
            var _deptRow = _db.PatientLoginEntries.Where(x => x.PatientId == info.PatientId
                                                        && DbFunctions.TruncateTime(x.LoginAttemptDate) == DbFunctions.TruncateTime(DateTime.Now))
                                                  .FirstOrDefault();
            if (_deptRow == null)
            {
                info.LoginAttempt = 1;
                _db.Entry(info).State = EntityState.Added;
                _effectRow = _db.SaveChanges();
                return info;
            }
            else if (_deptRow.LoginAttempt == 4)
            {
                return _deptRow;
            }
            else
            {
                _deptRow.LoginAttempt = _deptRow.LoginAttempt + 1;
                _deptRow.Locked = _deptRow.LoginAttempt == 4;
                _db.Entry(_deptRow).State = EntityState.Modified;
                _db.SaveChanges();
                return _deptRow;
            }
        }
        public List<PatientInfo> GetPatientDetailByRegistrationNumberSearch(string regNo)
        {
            _db = new upprbDbEntities();
            return _db.PatientInfoes.Include(x => x.Department).Where(x => x.RegistrationNumber.Contains(regNo)).ToList();
        }
        public List<PatientInfo> GetAllPatientDetail()
        {
            _db = new upprbDbEntities();
            return _db.PatientInfoes.Include(x => x.Department).Where(x => DbFunctions.TruncateTime(x.ValidUpto) >= DbFunctions.TruncateTime(DateTime.Now)).ToList();
        }
        public bool SavePatientLabReport(LabReport info)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            _db.Entry(info).State = EntityState.Added;
            _effectRow = _db.SaveChanges();
            return _effectRow > 0;
        }

        public List<LabReport> GetPatientLabReports(int patientId)
        {
            _db = new upprbDbEntities();
            return _db.LabReports.Include(x => x.PatientInfo).Where(x => x.PatientId == patientId).ToList();
        }

        public List<ReportModel> GetReportList(int patientId)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "LabReports";
            List<ReportModel> _newList = new List<ReportModel>();

            var labrepots = GetPatientLabReports(patientId);

            foreach (var report in labrepots)
            {
                ReportModel _newReport = new ReportModel();
                _newReport.Date = report.CreatedDate.Value;
                _newReport.FileName = report.ReportName;
                _newReport.ReportName = report.ReportName;
                _newList.Add(_newReport);
            }
            return _newList;
        }

        public List<State> GetStates()
        {
            _db = new upprbDbEntities();
            _db.Configuration.LazyLoadingEnabled = false;
            return _db.States.ToList();
        }
        public List<City> GetAllCities()
        {
            _db = new upprbDbEntities();
            _db.Configuration.LazyLoadingEnabled = false;
            return _db.Cities.ToList();
        }
        public List<Department> GetAllDepartment()
        {
            _db = new upprbDbEntities();
            _db.Configuration.LazyLoadingEnabled = false;
            return _db.Departments.ToList();
        }
        public List<City> GetCities(int stateId)
        {
            _db = new upprbDbEntities();
            _db.Configuration.LazyLoadingEnabled = false;
            return _db.Cities.Where(x => x.StateId == stateId).ToList();
        }
        public State GetStateByStateId(int stateId)
        {
            _db = new upprbDbEntities();
            _db.Configuration.LazyLoadingEnabled = false;
            return _db.States.Where(x => x.StateId == stateId).FirstOrDefault();
        }
        public City GetCitieByCItyId(int citiId)
        {
            _db = new upprbDbEntities();
            _db.Configuration.LazyLoadingEnabled = false;
            return _db.Cities.Where(x => x.CityId == citiId).FirstOrDefault();
        }

        public State GetStateIdByStateName(string stateName)
        {
            _db = new upprbDbEntities();
            _db.Configuration.LazyLoadingEnabled = false;
            return _db.States.Where(x => x.StateName.Equals(stateName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }
        public City GetCityIdByCItyName(string cityName)
        {
            _db = new upprbDbEntities();
            _db.Configuration.LazyLoadingEnabled = false;
            return _db.Cities.Where(x => x.CityName.Equals(cityName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public void DeletePatientInfoCRData(string crNumber)
        {
            _db = new upprbDbEntities();
            var deleteData = _db.PatientInfoCRClones.Where(x => x.CRNumber.Equals(crNumber)).FirstOrDefault();
            _db.PatientInfoCRClones.Remove(deleteData);
            _db.SaveChanges();
        }

        public PatientInfoCRClone GetPatientCloneDetailByCRNumber(string crNumber)
        {
            _db = new upprbDbEntities();
            return _db.PatientInfoCRClones.Where(x => x.CRNumber == crNumber).FirstOrDefault();
        }
        public List<PatientInfo> SyncHISFailedPatientList()
        {
            _db = new upprbDbEntities();
            return _db.PatientInfoes.Where(x => x.RenewalStatusHIS.ToUpper() != "S" || x.RegistrationStatusHIS.ToUpper() != "S").ToList();
        }
        public bool SavePatientMessage(PatientMessage message)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            _db.Entry(message).State = EntityState.Added;
            _effectRow = _db.SaveChanges();
            return true;
        }
        public int GetPatientMessageCount(int Id)
        {
            _db = new upprbDbEntities();
            return _db.PatientMessages.Where(x => x.PatientId == Id && x.HasRead == false).ToList().Count;
        }
        public List<PatientMessage> UpdateAndGetPatientMessageList(int Id)
        {
            _db = new upprbDbEntities();
            var result = _db.PatientMessages.Where(x => x.PatientId == Id && x.HasRead == false).ToList();
            result.ForEach(x =>
            {
                x.HasRead = true;
            });
            _db.SaveChanges();
            return _db.PatientMessages.Where(x => x.PatientId == Id).OrderByDescending(x => x.CreatedDate).Take(10).ToList();
        }
    }
}
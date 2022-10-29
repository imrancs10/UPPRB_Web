﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using System.Data.Entity;
using UPPRB_Web.Global;
using UPPRB_Web.Models.Masters;
using UPPRB_Web.Infrastructure;
using UPPRB_Web.Infrastructure.Utility;
using System.Threading.Tasks;

namespace UPPRB_Web.BAL.Masters
{
    public class DoctorDetails
    {
        UPPRB_WebEntities _db = null;
        public Enums.CrudStatus SaveDoctor(string doctorName, int deptId, string designation, string degree, string doctorDesc)
        {
            _db = new UPPRB_WebEntities();
            int _effectRow = 0;
            var _deptRow = _db.Doctors.Where(x => x.DoctorName.Equals(doctorName) && x.DepartmentID.Equals(deptId)).FirstOrDefault();
            if (_deptRow == null)
            {
                Doctor _newDoc = new Doctor();
                _newDoc.DoctorName = doctorName;
                _newDoc.DepartmentID = deptId;
                _newDoc.Degree = degree;
                _newDoc.Designation = designation;
                _newDoc.CreatedDate = DateTime.Now;
                _newDoc.Description = doctorDesc;
                _db.Entry(_newDoc).State = EntityState.Added;
                _effectRow = _db.SaveChanges();
                WebSession.DoctorId = _newDoc.DoctorID;
                return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
            }
            else
                return Enums.CrudStatus.DataAlreadyExist;
        }
        public Enums.CrudStatus EditDoctor(string doctorName, int deptId, int docId, string designation, string degree, string doctorDesc)
        {
            _db = new UPPRB_WebEntities();
            int _effectRow = 0;
            var _docRow = _db.Doctors.Where(x => x.DoctorID.Equals(docId)).FirstOrDefault();
            if (_docRow != null)
            {
                _docRow.DoctorName = doctorName;
                _docRow.DepartmentID = deptId;
                _docRow.Designation = designation;
                _docRow.Degree = degree;
                _docRow.ModifiedDate = DateTime.Now;
                _docRow.Description = doctorDesc;
                _db.Entry(_docRow).State = EntityState.Modified;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
            }
            else
                return Enums.CrudStatus.DataNotFound;
        }
        public Enums.CrudStatus UpdateDoctorImage(byte[] image, int doctorId)
        {
            _db = new UPPRB_WebEntities();
            int _effectRow = 0;
            var _doctorRow = _db.Doctors.Where(x => x.DoctorID.Equals(doctorId)).FirstOrDefault();
            if (_doctorRow != null)
            {
                _doctorRow.Image = image;
                _db.Entry(_doctorRow).State = EntityState.Modified;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
            }
            else
            {
                return Enums.CrudStatus.DataNotFound;
            }
        }
        public Enums.CrudStatus DeleteDoctor(int docId)
        {
            _db = new UPPRB_WebEntities();
            int _effectRow = 0;
            var _docRow = _db.Doctors.Where(x => x.DoctorID.Equals(docId)).FirstOrDefault();
            if (_docRow != null)
            {
                _db.Doctors.Remove(_docRow);
                //_db.Entry(_deptRow).State = EntityState.Deleted;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Deleted : Enums.CrudStatus.NotDeleted;
            }
            else
                return Enums.CrudStatus.DataNotFound;
        }
        public List<DoctorModel> DoctorList(int deptId = 0)
        {
            _db = new UPPRB_WebEntities();
            var _list = (from doc in _db.Doctors
                         from dept in _db.Departments.Where(x => x.DepartmentID.Equals(doc.DepartmentID))
                         orderby dept.DepartmentName
                         where deptId == 0 || deptId.Equals(doc.DepartmentID)
                         select new DoctorModel
                         {
                             DoctorName = doc.DoctorName,
                             DepartmentId = dept.DepartmentID,
                             DoctorId = doc.DoctorID,
                             DepartmentName = dept.DepartmentName,
                             Description = doc.Description,
                             //Image = doc.Image,
                             Degree = string.IsNullOrEmpty(doc.Degree) ? string.Empty : doc.Degree,
                             Designation = string.IsNullOrEmpty(doc.Designation) ? string.Empty : doc.Designation
                         }).OrderBy(x => x.DoctorId).ToList();
            return _list != null ? _list : new List<DoctorModel>();
        }

        public DoctorModel GetDoctorById(int deptId)
        {
            _db = new UPPRB_WebEntities();
            var _deptRow = _db.Doctors.Where(x => x.DoctorID.Equals(deptId)).FirstOrDefault();
            if (_deptRow != null)
            {
                var dep = new DoctorModel()
                {
                    Image = _deptRow.Image
                };
                return dep;
            }
            return null;
        }
        public List<DoctorTypeModel> GetDoctorTypeList()
        {
            _db = new UPPRB_WebEntities();
            var _list = (from doc in _db.DoctorTypes
                         select new DoctorTypeModel
                         {
                             Id = doc.Id,
                             DoctorType = doc.DoctorType1,
                         }).OrderBy(x => x.DoctorType).ToList();
            return _list != null ? _list : new List<DoctorTypeModel>();
        }
        public Enums.CrudStatus UpdateDoctorType(int docId, int doctortype)
        {
            _db = new UPPRB_WebEntities();
            int _effectRow = 0;
            var _docRow = _db.Doctors.Where(x => x.DoctorID.Equals(docId)).FirstOrDefault();
            if (_docRow != null)
            {
                _docRow.DoctorTypeId = doctortype;
                _docRow.ModifiedDate = DateTime.Now;
                _db.Entry(_docRow).State = EntityState.Modified;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
            }
            else
                return Enums.CrudStatus.DataNotFound;
        }
        public IEnumerable<object> GetDoctorLeaveList(int doctorId)
        {
            _db = new UPPRB_WebEntities();
            return (from leave in _db.DoctorLeaves.Where(x => x.DoctorId.Equals(doctorId))
                    select new
                    {
                        leave.DoctorId,
                        leave.Doctor.DoctorName,
                        leave.Doctor.DepartmentID,
                        leave.Doctor.Department.DepartmentName,
                        leave.LeaveDate
                    }).OrderBy(x => x.LeaveDate).ThenBy(x => x.DoctorName).ToList();

        }
        public Enums.CrudStatus SaveDoctorLeave(int doctorId, DateTime leaveDate)
        {
            int a = 100;
            int b = 200;
            long tatal;
            tatal = a + b;
            if (doctorId < 1)
            {
                return Enums.CrudStatus.InvalidPostedData;
            }
            else if (leaveDate.Date < DateTime.Now.Date)
            {
                return Enums.CrudStatus.InvalidPastDate;
            }
            else
            {
                _db = new UPPRB_WebEntities();
                int _effectRow = 0;
                var _deptRow = _db.DoctorLeaves.Where(x => x.DoctorId.Equals(doctorId) && x.LeaveDate.Equals(leaveDate)).FirstOrDefault();
                if (_deptRow == null)
                {
                    DoctorLeave _newDoc = new DoctorLeave();
                    _newDoc.DoctorId = doctorId;
                    _newDoc.LeaveDate = leaveDate;
                    _newDoc.CreatedDate = DateTime.Now;
                    _db.Entry(_newDoc).State = EntityState.Added;
                    _effectRow = _db.SaveChanges();
                    if (_effectRow > 0)
                    {
                        var appointments = _db.AppointmentInfoes.Where(x => x.DoctorId.Equals(doctorId)
                                                                        && !x.IsCancelled.Value
                                                                        && DbFunctions.TruncateTime(x.AppointmentDateFrom) == DbFunctions.TruncateTime(leaveDate)
                                                                      ).ToList();
                        if (appointments.Count > 0)
                        {
                            foreach (AppointmentInfo appointment in appointments)
                            {
                                Task mail = SendEmail(appointment, leaveDate);
                                appointment.CancelDate = DateTime.Now;
                                appointment.IsCancelled = true;
                                appointment.ModifiedBy = appointment.PatientId;
                                appointment.CancelReason = WebSession.AutoCancelMessage == string.Empty ? "Auto cancel-Doctor on leave" : WebSession.AutoCancelMessage;
                                _db.Entry(appointment).State = EntityState.Modified;
                                _db.SaveChanges();
                            }
                        }
                        return Enums.CrudStatus.Saved;
                    }
                    else
                    {
                        return Enums.CrudStatus.NotSaved;
                    }
                }
                else
                    return Enums.CrudStatus.DataAlreadyExist;
            }
        }

        private async Task SendEmail(AppointmentInfo patient, DateTime leaveDate)
        {
            await Task.Run(() =>
            {
                //Send Email
                Message msg = new Message()
                {
                    MessageTo = patient.PatientInfo.Email,
                    MessageNameTo = patient.PatientInfo.FirstName + " " + patient.PatientInfo.MiddleName + (string.IsNullOrWhiteSpace(patient.PatientInfo.MiddleName) ? string.Empty : " ") + patient.PatientInfo.LastName,
                    Subject = "Appointment Booking Confirmation",
                    Body = EmailHelper.GetDoctorAbsentEmail(patient.PatientInfo.FirstName, patient.PatientInfo.MiddleName, patient.PatientInfo.LastName, patient.Doctor.DoctorName, leaveDate, patient.Doctor.Department.DepartmentName)
                };
                ISendMessageStrategy sendMessageStrategy = new SendMessageStrategyForEmail(msg);
                sendMessageStrategy.SendMessages();

                //Send SMS
                msg.Body = "Hi " + string.Format("{0} {1}", patient.PatientInfo.FirstName, patient.PatientInfo.LastName) + "\nThis just to inform you. Doctor " + patient.Doctor.DoctorName + " is not available on " + leaveDate + "so your below appointment is cancelled.\n Regards:\n Patient Portal(RMLHIMS)";
                msg.MessageTo = patient.PatientInfo.MobileNumber;
                msg.MessageType = MessageType.Appointment;
                sendMessageStrategy = new SendMessageStrategyForSMS(msg);
                sendMessageStrategy.SendMessages();
            });
        }
    }
}
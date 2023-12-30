using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using System.Data.Entity;
using UPPRB_Web.Global;
using UPPRB_Web.Models.Masters;
using iTextSharp.text.pdf;

namespace UPPRB_Web.BAL.Masters
{
    public class AdminDetails
    {
        upprbDbEntities _db = null;
        public Enums.CrudStatus SavePopularRecruitment(PopularRecruitment recruitment)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            if (recruitment.Id == 0)
                _db.Entry(recruitment).State = EntityState.Added;
            else
            {
                var _deptRow = _db.PopularRecruitments.Where(x => x.Id.Equals(recruitment.Id)).FirstOrDefault();
                if (_deptRow != null)
                {
                    _deptRow.fileURL = recruitment.fileURL;
                    _deptRow.RecruitmentEndDate = recruitment.RecruitmentEndDate;
                    _deptRow.RecruitmentStartDate = recruitment.RecruitmentStartDate;
                    _deptRow.RecruitmentName = recruitment.RecruitmentName;
                    _deptRow.RecruitmentSubject = recruitment.RecruitmentSubject;
                    _deptRow.NoOfSeat = recruitment.NoOfSeat;
                    _deptRow.is_active = recruitment.is_active;
                    _db.Entry(_deptRow).State = EntityState.Modified;
                    _effectRow = _db.SaveChanges();
                    return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
                }
            }
            _effectRow = _db.SaveChanges();
            return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }
        public Enums.CrudStatus SaveNotice(Notice notice)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            if (notice.Id == 0)
                _db.Entry(notice).State = EntityState.Added;
            else
            {
                var _deptRow = _db.Notices.Where(x => x.Id.Equals(notice.Id)).FirstOrDefault();
                if (_deptRow != null)
                {
                    _deptRow.fileURL = notice.fileURL;
                    _deptRow.Subject = notice.Subject;
                    _deptRow.NoticeDate = notice.NoticeDate;
                    _deptRow.NoticeCategoryId = notice.NoticeCategoryId;
                    _deptRow.EntryTypeId = notice.EntryTypeId;
                    _deptRow.filename = !string.IsNullOrEmpty(notice.filename) ? notice.filename : _deptRow.filename;
                    _deptRow.IsNew = notice.IsNew;
                    _deptRow.NoticeType = notice.NoticeType;
                    _db.Entry(_deptRow).State = EntityState.Modified;
                    _effectRow = _db.SaveChanges();
                    return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
                }
            }
            _effectRow = _db.SaveChanges();
            return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }
        public Enums.CrudStatus SavePACEntry(PACEntry notice)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            if (notice.Id == 0)
            {
                var _deptRow = _db.PACEntries.OrderByDescending(x => x.Id).FirstOrDefault();
                var lastPACNumber = _deptRow.PACNumber;
                if (lastPACNumber.Substring(5, 4) == DateTime.UtcNow.Year.ToString())
                {
                    var lastNumber = Convert.ToInt32(lastPACNumber.Substring(9, 4));
                    int addPad = 4 - (lastNumber + 1).ToString().Length;
                    var padNumber = (lastNumber + 1).ToString().PadLeft((lastNumber + 1).ToString().Length + addPad, '0');
                    notice.PACNumber = "CSPAC" + DateTime.UtcNow.Year.ToString() + padNumber;
                }
                else
                {
                    notice.PACNumber = "CSPAC" + DateTime.UtcNow.Year.ToString() + "0001";
                }
                _db.Entry(notice).State = EntityState.Added;

            }
            else
            {
                var _deptRow = _db.PACEntries.Where(x => x.Id.Equals(notice.Id)).FirstOrDefault();
                if (_deptRow != null)
                {
                    _deptRow.Address = notice.Address;
                    _deptRow.FIRDate = notice.FIRDate;
                    _deptRow.PublishDate = notice.PublishDate;
                    _deptRow.AccusedName = notice.AccusedName;
                    _deptRow.District_Id = notice.District_Id;
                    _deptRow.FileUploadName = !string.IsNullOrEmpty(notice.FileUploadName) ? notice.FileUploadName : _deptRow.FileUploadName;
                    _deptRow.ExamineCenterName = notice.ExamineCenterName;
                    _deptRow.Solver_Name = notice.Solver_Name;
                    _deptRow.FIRDetails = notice.FIRDetails;
                    _deptRow.FIRNo = notice.FIRNo;
                    _deptRow.PS_Id = notice.PS_Id;
                    _deptRow.Range_Id = notice.Range_Id;
                    _deptRow.State_Id = notice.State_Id;
                    _deptRow.Zone_Id = notice.Zone_Id;
                    _deptRow.recruitement_type = notice.recruitement_type;
                    _deptRow.Remark = notice.Remark;
                    _deptRow.CenterStatus = notice.CenterStatus;
                    _db.Entry(_deptRow).State = EntityState.Modified;
                    _effectRow = _db.SaveChanges();
                    return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
                }
            }
            _effectRow = _db.SaveChanges();
            return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }

        public bool IsDuplicateFIR(PACEntry pac)
        {
            _db = new upprbDbEntities();
            var _list = (from en in _db.PACEntries
                         where en.FIRNo == pac.FIRNo && en.District_Id == pac.District_Id
                         && en.PS_Id == pac.PS_Id && en.ExamineCenterName == pac.ExamineCenterName
                         && en.AccusedName == pac.AccusedName
                         && ((pac.Id != 0 && en.Id != pac.Id) || pac.Id == 0)
                         select new
                         {
                             id = en.Id
                         }).ToList();
            return _list.Any();
        }
        public Enums.CrudStatus SavePromotionEntry(PromotionDetail notice)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            if (notice.Id == 0)
                _db.Entry(notice).State = EntityState.Added;
            else
            {
                //var _deptRow = _db.Notices.Where(x => x.Id.Equals(notice.Id)).FirstOrDefault();
                //if (_deptRow != null)
                //{
                //    _deptRow.fileURL = notice.fileURL;
                //    _deptRow.Subject = notice.Subject;
                //    _deptRow.NoticeDate = notice.NoticeDate;
                //    _deptRow.NoticeCategoryId = notice.NoticeCategoryId;
                //    _deptRow.EntryTypeId = notice.EntryTypeId;
                //    _deptRow.filename = !string.IsNullOrEmpty(notice.filename) ? notice.filename : _deptRow.filename;
                //    _deptRow.IsNew = notice.IsNew;
                //    _deptRow.NoticeType = notice.NoticeType;
                //    _db.Entry(_deptRow).State = EntityState.Modified;
                //    _effectRow = _db.SaveChanges();
                //    return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
                //}
            }
            _effectRow = _db.SaveChanges();
            return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }
        public Enums.CrudStatus SaveDirectRecruitementEntry(DirectRecruitementDetail notice)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            if (notice.Id == 0)
                _db.Entry(notice).State = EntityState.Added;
            else
            {
                //var _deptRow = _db.Notices.Where(x => x.Id.Equals(notice.Id)).FirstOrDefault();
                //if (_deptRow != null)
                //{
                //    _deptRow.fileURL = notice.fileURL;
                //    _deptRow.Subject = notice.Subject;
                //    _deptRow.NoticeDate = notice.NoticeDate;
                //    _deptRow.NoticeCategoryId = notice.NoticeCategoryId;
                //    _deptRow.EntryTypeId = notice.EntryTypeId;
                //    _deptRow.filename = !string.IsNullOrEmpty(notice.filename) ? notice.filename : _deptRow.filename;
                //    _deptRow.IsNew = notice.IsNew;
                //    _deptRow.NoticeType = notice.NoticeType;
                //    _db.Entry(_deptRow).State = EntityState.Modified;
                //    _effectRow = _db.SaveChanges();
                //    return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
                //}
            }
            _effectRow = _db.SaveChanges();
            return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }

        public Enums.CrudStatus ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            var userId = UserData.UserId;
            if (!string.IsNullOrEmpty(oldPassword) && !string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(confirmPassword))
            {
                var existingData = _db.AdminUsers.FirstOrDefault(x => x.Id == userId && x.Password == oldPassword);
                if (existingData == null)
                    return Enums.CrudStatus.DataNotFound;
                else
                {
                    existingData.Password = newPassword;
                    _effectRow = _db.SaveChanges();
                }
            }
            return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }

        public Enums.CrudStatus SavePSEntry(PSMaster notice)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            if (notice.PSId == 0)
                _db.Entry(notice).State = EntityState.Added;
            else
            {
                var _deptRow = _db.PSMasters.Where(x => x.PSId.Equals(notice.PSId)).FirstOrDefault();
                if (_deptRow != null)
                {
                    _deptRow.PSName = notice.PSName;
                    _deptRow.DistrictId = notice.DistrictId;
                    _db.Entry(_deptRow).State = EntityState.Modified;
                    _effectRow = _db.SaveChanges();
                    return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
                }
            }
            _effectRow = _db.SaveChanges();
            return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }

        public List<PSEntryModel> GetPSEntry()
        {
            _db = new upprbDbEntities();
            var _list = (from lookEntry in _db.PSMasters
                         join dis in _db.DistrictMasters on lookEntry.DistrictId equals dis.DistrictId
                         select new PSEntryModel
                         {
                             DistrictId = lookEntry.DistrictId,
                             PSName = lookEntry.PSName,
                             DistrictName = dis.DistrictName,
                             PSId = lookEntry.PSId,
                         }).OrderBy(x => x.DistrictName).ToList();
            return _list != null ? _list : new List<PSEntryModel>();
        }
        public List<MedalEntryModel> GetMedalEntry()
        {
            _db = new upprbDbEntities();
            var _list = (from lookEntry in _db.MedalDetails
                         join dis in _db.Lookups on lookEntry.MedalCategoryId equals dis.LookupId
                         select new MedalEntryModel
                         {
                             FileName = lookEntry.FileName,
                             GivenBy = lookEntry.GivenBy,
                             Id = lookEntry.Id,
                             IsActive = lookEntry.IsActive,
                             MedalCategoryId = lookEntry.MedalCategoryId,
                             MedalCategoryName = dis.LookupName,
                             MedalDescription = lookEntry.MedalDescription,
                             MedalGivenDate = lookEntry.MedalGivenDate,
                             ToWhom = lookEntry.ToWhom,
                             UpdatedDate = lookEntry.UpdatedDate
                         }).OrderByDescending(x => x.MedalGivenDate).ToList();
            return _list != null ? _list : new List<MedalEntryModel>();
        }
        public List<AdminUserModel> GetAdminUser()
        {
            _db = new upprbDbEntities();
            var _list = (from lookEntry in _db.AdminUsers
                         join dis in _db.Roles on lookEntry.RoleId equals dis.RoleId
                         select new AdminUserModel
                         {
                             Id = lookEntry.Id,
                             IsActive = lookEntry.IsActive,
                             EmailID = lookEntry.EmailID,
                             MobileNumber = lookEntry.MobileNumber,
                             Name = lookEntry.Name,
                             RoleId = lookEntry.RoleId,
                             RoleName = dis.RoleName,
                             UserName = lookEntry.UserName
                         }).OrderByDescending(x => x.Name).ToList();
            return _list != null ? _list : new List<AdminUserModel>();
        }
        public Enums.CrudStatus DeletePSEntry(int Id)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            var _deptRow = _db.PSMasters.Where(x => x.PSId.Equals(Id)).FirstOrDefault();
            if (_deptRow != null)
            {
                _db.PSMasters.Remove(_deptRow);
                _db.Entry(_deptRow).State = EntityState.Deleted;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Deleted : Enums.CrudStatus.NotDeleted;
            }
            else
                return Enums.CrudStatus.DataNotFound;
        }
        public Enums.CrudStatus DeleteMedalDetailEntry(int Id)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            var _deptRow = _db.MedalDetails.Where(x => x.Id.Equals(Id)).FirstOrDefault();
            if (_deptRow != null)
            {
                _db.MedalDetails.Remove(_deptRow);
                _db.Entry(_deptRow).State = EntityState.Deleted;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Deleted : Enums.CrudStatus.NotDeleted;
            }
            else
                return Enums.CrudStatus.DataNotFound;
        }

        public Enums.CrudStatus DeleteUserDetailEntry(int Id)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            var _deptRow = _db.AdminUsers.Where(x => x.Id.Equals(Id)).FirstOrDefault();
            if (_deptRow != null)
            {
                _db.AdminUsers.Remove(_deptRow);
                _db.Entry(_deptRow).State = EntityState.Deleted;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Deleted : Enums.CrudStatus.NotDeleted;
            }
            else
                return Enums.CrudStatus.DataNotFound;
        }
        public Enums.CrudStatus SaveMedalEntry(MedalDetail notice)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            if (notice.Id == 0)
            {
                _db.Entry(notice).State = EntityState.Added;
            }
            else
            {
                var _deptRow = _db.MedalDetails.Where(x => x.Id.Equals(notice.Id)).FirstOrDefault();
                if (_deptRow != null)
                {
                    _deptRow.FileName = !string.IsNullOrEmpty(notice.FileName) ? notice.FileName : _deptRow.FileName;
                    _deptRow.UpdatedDate = notice.UpdatedDate;
                    _deptRow.MedalGivenDate = notice.MedalGivenDate;
                    _deptRow.MedalDescription = notice.MedalDescription;
                    _deptRow.MedalCategoryId = notice.MedalCategoryId;
                    _deptRow.ToWhom = notice.ToWhom;
                    _deptRow.GivenBy = notice.GivenBy;
                    _db.Entry(_deptRow).State = EntityState.Modified;
                    _effectRow = _db.SaveChanges();
                    return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
                }
            }
            _effectRow = _db.SaveChanges();
            return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }
        public Enums.CrudStatus SaveNewUserEntry(AdminUser notice)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            if (notice.Id == 0)
            {
                _db.Entry(notice).State = EntityState.Added;
            }
            else
            {
                var _deptRow = _db.AdminUsers.Where(x => x.Id.Equals(notice.Id)).FirstOrDefault();
                if (_deptRow != null)
                {
                    _deptRow.MobileNumber = notice.MobileNumber;
                    _deptRow.UserName = notice.UserName;
                    _deptRow.Name = notice.Name;
                    _deptRow.EmailID = notice.EmailID;
                    _deptRow.RoleId = notice.RoleId;
                    _deptRow.IsActive = notice.IsActive;
                    _db.Entry(_deptRow).State = EntityState.Modified;
                    _effectRow = _db.SaveChanges();
                    return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
                }
            }
            _effectRow = _db.SaveChanges();
            return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }
        public Enums.CrudStatus DeleteEventCalender(int Id)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            var _deptRow = _db.EventCalenders.Where(x => x.Id.Equals(Id)).FirstOrDefault();
            if (_deptRow != null)
            {
                _db.EventCalenders.Remove(_deptRow);
                _db.Entry(_deptRow).State = EntityState.Deleted;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Deleted : Enums.CrudStatus.NotDeleted;
            }
            else
                return Enums.CrudStatus.DataNotFound;
        }
        public Enums.CrudStatus SaveEventCalender(EventCalender notice)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            if (notice.Id == 0)
            {
                _db.Entry(notice).State = EntityState.Added;
            }
            else
            {
                var _deptRow = _db.EventCalenders.Where(x => x.Id.Equals(notice.Id)).FirstOrDefault();
                if (_deptRow != null)
                {
                    _deptRow.FileName = !string.IsNullOrEmpty(notice.FileName) ? notice.FileName : _deptRow.FileName;
                    _deptRow.UpdatedDate = notice.UpdatedDate;
                    _deptRow.EventDate = notice.EventDate;
                    _deptRow.EventDescription = notice.EventDescription;
                    _deptRow.EventTitle = notice.EventTitle;
                    _db.Entry(_deptRow).State = EntityState.Modified;
                    _effectRow = _db.SaveChanges();
                    return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
                }
            }
            _effectRow = _db.SaveChanges();
            return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }
        public List<EventCalenderModel> GetEventCalender()
        {
            _db = new upprbDbEntities();
            var _list = (from lookEntry in _db.EventCalenders
                         where lookEntry.EventDate != null
                         select new EventCalenderModel
                         {
                             FileName = lookEntry.FileName,
                             EventTitle = lookEntry.EventTitle,
                             Id = lookEntry.Id,
                             IsActive = lookEntry.IsActive,
                             EventDescription = lookEntry.EventDescription,
                             EventDate = lookEntry.EventDate,
                             UpdatedDate = lookEntry.UpdatedDate
                         }).OrderByDescending(x => x.EventDate).ToList();
            return _list != null ? _list : new List<EventCalenderModel>();
        }
        public Enums.CrudStatus SaveFAQEntry(FAQDetail notice)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            if (notice.Id == 0)
                _db.Entry(notice).State = EntityState.Added;
            else
            {
                var _deptRow = _db.FAQDetails.Where(x => x.Id.Equals(notice.Id)).FirstOrDefault();
                if (_deptRow != null)
                {
                    _deptRow.FAQ_Question = notice.FAQ_Question;
                    _deptRow.FAQ_Answer = notice.FAQ_Answer;
                    _db.Entry(_deptRow).State = EntityState.Modified;
                    _effectRow = _db.SaveChanges();
                    return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
                }
            }
            _effectRow = _db.SaveChanges();
            return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }

        public List<FAQEntryModel> GetFAQEntry()
        {
            _db = new upprbDbEntities();
            var _list = (from lookEntry in _db.FAQDetails
                         where lookEntry.IsActive == true
                         select new FAQEntryModel
                         {
                             Id = lookEntry.Id,
                             CreatedDate = lookEntry.CreatedDate,
                             FAQ_Answer = lookEntry.FAQ_Answer,
                             FAQ_Question = lookEntry.FAQ_Question,
                             IsActive = lookEntry.IsActive,
                         }).OrderBy(x => x.FAQ_Question).ToList();
            return _list != null ? _list : new List<FAQEntryModel>();
        }
        public Enums.CrudStatus DeleteFAQEntry(int Id)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            var _deptRow = _db.FAQDetails.Where(x => x.Id.Equals(Id)).FirstOrDefault();
            if (_deptRow != null)
            {
                _deptRow.IsActive = false;
                //_db.FAQDetails.Remove(_deptRow);
                _db.Entry(_deptRow).State = EntityState.Modified;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Deleted : Enums.CrudStatus.NotDeleted;
            }
            else
                return Enums.CrudStatus.DataNotFound;
        }
        //public Enums.CrudStatus EditDept(string deptName, int deptId, string deptUrl,string  deptDesc)
        //{
        //    _db = new upprbDbEntities();
        //    int _effectRow = 0;
        //    var _deptRow = _db.Departments.Where(x => x.DepartmentID.Equals(deptId)).FirstOrDefault();
        //    if (_deptRow != null)
        //    {
        //        _deptRow.DepartmentName = deptName;
        //        _deptRow.DepartmentUrl = deptUrl;
        //        _deptRow.Description = deptDesc;
        //        _db.Entry(_deptRow).State = EntityState.Modified;
        //        _effectRow = _db.SaveChanges();
        //        return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
        //    }
        //    else
        //        return Enums.CrudStatus.DataNotFound;
        //}
        //public Enums.CrudStatus UpdateDeptImage(byte[] image, int deptId)
        //{
        //    _db = new upprbDbEntities();
        //    int _effectRow = 0;
        //    var _deptRow = _db.Departments.Where(x => x.DepartmentID.Equals(deptId)).FirstOrDefault();
        //    if (_deptRow != null)
        //    {
        //        _deptRow.Image = image;
        //        _db.Entry(_deptRow).State = EntityState.Modified;
        //        _effectRow = _db.SaveChanges();
        //        return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
        //    }
        //    else
        //        return Enums.CrudStatus.DataNotFound;
        //}
        //public Enums.CrudStatus DeleteDept(int deptId)
        //{
        //    _db = new upprbDbEntities();
        //    int _effectRow = 0;
        //    var _deptRow = _db.Departments.Where(x => x.DepartmentID.Equals(deptId)).FirstOrDefault();
        //    if (_deptRow != null)
        //    {
        //        _db.Departments.Remove(_deptRow);
        //        //_db.Entry(_deptRow).State = EntityState.Deleted;
        //        _effectRow = _db.SaveChanges();
        //        return _effectRow > 0 ? Enums.CrudStatus.Deleted : Enums.CrudStatus.NotDeleted;
        //    }
        //    else
        //        return Enums.CrudStatus.DataNotFound;
        //}

        //public List<DepartmentModel> DepartmentList()
        //{
        //    _db = new upprbDbEntities();
        //    var _list = (from dept in _db.Departments
        //                 select new DepartmentModel
        //                 {
        //                     DeparmentName = dept.DepartmentName,
        //                     DepartmentId = dept.DepartmentID,
        //                     DepartmentUrl = dept.DepartmentUrl,
        //                     Description = dept.Description,
        //                     //Image = dept.Image
        //                     //ImageUrl= string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(dept.Image))
        //                 }).ToList();
        //    return _list != null ? _list : new List<DepartmentModel>();
        //}
        //public List<MasterLookupModel> GetMastersData()
        //{
        //    _db = new upprbDbEntities();
        //    var _list = (from dept in _db.MasterLookups
        //                 select new MasterLookupModel
        //                 {
        //                     Name = dept.Name,
        //                     Id = dept.Id,
        //                     Value = dept.Value
        //                 }).ToList();
        //    return _list != null ? _list : new List<MasterLookupModel>();
        //}

        //public DepartmentModel GetDeparmentById(int deptId)
        //{
        //    _db = new upprbDbEntities();
        //    int _effectRow = 0;
        //    var _deptRow = _db.Departments.Where(x => x.DepartmentID.Equals(deptId)).FirstOrDefault();
        //    if (_deptRow != null)
        //    {
        //        var dep = new DepartmentModel()
        //        {
        //            DeparmentName = _deptRow.DepartmentName,
        //            DepartmentId = _deptRow.DepartmentID,
        //            DepartmentUrl = _deptRow.DepartmentUrl,
        //            Description = _deptRow.Description,
        //            Image = _deptRow.Image
        //        };
        //        return dep;
        //    }
        //    return null;
        //}

        //public Enums.CrudStatus SaveMasterLookup(string name, string value)
        //{
        //    _db = new upprbDbEntities();
        //    int _effectRow = 0;
        //    var _deptRow = _db.MasterLookups.Where(x => x.Name.Equals(name)).FirstOrDefault();
        //    if (_deptRow == null)
        //    {
        //        MasterLookup _newDept = new MasterLookup();
        //        _newDept.Name = name;
        //        _newDept.Value = value;
        //        _db.Entry(_newDept).State = EntityState.Added;
        //        _effectRow = _db.SaveChanges();
        //        return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        //    }
        //    else
        //        return Enums.CrudStatus.DataAlreadyExist;
        //}

        //public Enums.CrudStatus EditMasterLookup(string name, string value, int deptId)
        //{
        //    _db = new upprbDbEntities();
        //    int _effectRow = 0;
        //    var _deptRow = _db.MasterLookups.Where(x => x.Id.Equals(deptId)).FirstOrDefault();
        //    if (_deptRow != null)
        //    {
        //        _deptRow.Name = name;
        //        _deptRow.Value = value;
        //        _db.Entry(_deptRow).State = EntityState.Modified;
        //        _effectRow = _db.SaveChanges();
        //        return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
        //    }
        //    else
        //        return Enums.CrudStatus.DataNotFound;
        //}
        //public Enums.CrudStatus DeleteMasterLookup(int deptId)
        //{
        //    _db = new upprbDbEntities();
        //    int _effectRow = 0;
        //    var _deptRow = _db.MasterLookups.Where(x => x.Id.Equals(deptId)).FirstOrDefault();
        //    if (_deptRow != null)
        //    {
        //        _db.MasterLookups.Remove(_deptRow);
        //        //_db.Entry(_deptRow).State = EntityState.Deleted;
        //        _effectRow = _db.SaveChanges();
        //        return _effectRow > 0 ? Enums.CrudStatus.Deleted : Enums.CrudStatus.NotDeleted;
        //    }
        //    else
        //        return Enums.CrudStatus.DataNotFound;
        //}
    }
}
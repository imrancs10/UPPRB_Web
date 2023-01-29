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
                if (lastPACNumber.Substring(3, 4) == DateTime.UtcNow.Year.ToString())
                {
                    var lastNumber = Convert.ToInt32(lastPACNumber.Substring(7, 4));
                    int addPad = 4 - (lastNumber + 1).ToString().Length;
                    var padNumber = (lastNumber + 1).ToString().PadLeft((lastNumber + 1).ToString().Length + addPad, '0');
                    notice.PACNumber = "PAC" + DateTime.UtcNow.Year.ToString() + padNumber;
                }
                else
                {
                    notice.PACNumber = "PAC" + DateTime.UtcNow.Year.ToString() + "0001";
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
                    _deptRow.FIRDetails = notice.FIRDetails;
                    _deptRow.FIRNo = notice.FIRNo;
                    _deptRow.PS_Id = notice.PS_Id;
                    _deptRow.Range_Id = notice.Range_Id;
                    _deptRow.State_Id = notice.State_Id;
                    _deptRow.Zone_Id = notice.Zone_Id;
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
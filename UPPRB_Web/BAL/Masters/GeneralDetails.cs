using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using System.Data.Entity;
using UPPRB_Web.Global;
using UPPRB_Web.Models.Masters;
using WebActivatorEx;

namespace UPPRB_Web.BAL.Masters
{
    public class GeneralDetails
    {
        upprbDbEntities _db = null;
        public List<NoticeModel> GetEntryType()
        {
            _db = new upprbDbEntities();
            var _list = (from lookEntry in _db.Lookups
                         where lookEntry.LookupType == "UploadType" && lookEntry.IsActive == true
                         select new NoticeModel
                         {
                             EntryTypeId = lookEntry.LookupId,
                             EntryTypeName = lookEntry.LookupName,
                             EntryTypeDisplayName = lookEntry.LookupNameImmutable
                         }).OrderBy(x => x.EntryTypeName).ToList();
            return _list != null ? _list : new List<NoticeModel>();
        }
        public List<NoticeModel> GetNoticeDetail(int? noticeTypeId = null, int? categoryId = null, int? entryTypeId = null, int? noticeId = null)
        {
            _db = new upprbDbEntities();
            var _list = (from not in _db.Notices
                         join lookEntry in _db.Lookups on not.EntryTypeId equals lookEntry.LookupId into lookEntry1
                         from lookEntry2 in lookEntry1.DefaultIfEmpty()
                         join lookNoticeType in _db.Lookups on not.NoticeType equals lookNoticeType.LookupId into lookNoticeType1
                         from lookNoticeType2 in lookNoticeType1.DefaultIfEmpty()
                         join lookNotCategory in _db.Lookups on not.NoticeCategoryId equals lookNotCategory.LookupId into lookNotCategory1
                         from lookNotCategory2 in lookNotCategory1.DefaultIfEmpty()
                         where (noticeTypeId == null || (noticeTypeId != null && not.NoticeType == noticeTypeId))
                         && (categoryId == null || (categoryId != null && not.NoticeCategoryId == categoryId))
                          && (entryTypeId == null || (entryTypeId != null && not.EntryTypeId == entryTypeId))
                           && (noticeId == null || (noticeId != null && not.Id == noticeId))
                         select new NoticeModel
                         {
                             filename = not.filename != null ? not.filename : "",
                             CreatedBy = not.CreatedBy,
                             CreatedDate = not.CreatedDate,
                             fileURL = not.fileURL != null ? not.fileURL : "",
                             Id = not.Id,
                             NoticeCategoryId = not.NoticeCategoryId,
                             NoticeDate = not.NoticeDate,
                             EntryTypeId = not.EntryTypeId,
                             NoticeType = not.NoticeType,
                             Subject = not.Subject,
                             IsNew = not.IsNew,
                             EntryTypeName = lookEntry2 != null ? lookEntry2.LookupName : "",
                             NoticeCategoryName = lookNotCategory2 != null ? lookNotCategory2.LookupName : "",
                             NoticeTypeName = lookNoticeType2 != null ? lookNoticeType2.LookupName : ""
                         }).OrderByDescending(x => x.NoticeDate).ToList();
            return _list != null ? _list : new List<NoticeModel>();
        }
        public List<NoticeModel> GetLatestEventDetail()
        {
            _db = new upprbDbEntities();
            var currentDate = DateTime.Now;
            var _list = (from not in _db.Notices
                         join look in _db.Lookups on not.EntryTypeId equals look.LookupId
                         where currentDate >= not.NoticeDate && look.LookupType == "UploadType" && look.LookupName == "Notice"
                         select new NoticeModel
                         {
                             filename = not.filename,
                             CreatedBy = not.CreatedBy,
                             CreatedDate = not.CreatedDate,
                             fileURL = not.fileURL,
                             Id = not.Id,
                             NoticeCategoryId = not.NoticeCategoryId,
                             NoticeDate = not.NoticeDate,
                             EntryTypeId = not.EntryTypeId,
                             NoticeType = not.NoticeType,
                             Subject = not.Subject,
                             IsNew = not.IsNew
                         }).OrderByDescending(x => x.NoticeDate).Skip(1).Take(7).ToList();
            return _list != null ? _list : new List<NoticeModel>();
        }
        public NoticeModel GetHighlightedNoticeDetail()
        {
            _db = new upprbDbEntities();
            var currentDate = DateTime.Now;
            var _list = (from not in _db.Notices
                         join look in _db.Lookups on not.EntryTypeId equals look.LookupId
                         where currentDate >= not.NoticeDate && look.LookupType == "UploadType" && look.LookupName == "Notice"
                         select new NoticeModel
                         {
                             filename = not.filename,
                             CreatedBy = not.CreatedBy,
                             CreatedDate = not.CreatedDate,
                             fileURL = not.fileURL,
                             Id = not.Id,
                             NoticeCategoryId = not.NoticeCategoryId,
                             NoticeDate = not.NoticeDate,
                             EntryTypeId = not.EntryTypeId,
                             NoticeType = not.NoticeType,
                             Subject = not.Subject,
                             IsNew = not.IsNew
                         }).OrderByDescending(x => x.NoticeDate).FirstOrDefault();
            return _list != null ? _list : new NoticeModel();
        }

        public List<NoticeTypeModel> GetNoticeHirarchyDetail()
        {
            _db = new upprbDbEntities();
            var _list = (from not in _db.Lookups
                         join parent in _db.Lookups on not.ParentLookupId equals parent.LookupId
                         where not.IsActive == true && not.LookupType == "NoticeType" && parent.LookupType == "UploadType" && parent.LookupName == "Notice"
                         select new NoticeTypeModel
                         {
                             LookupId = not.LookupId,
                             LookupName = not.LookupName,
                             NoticeCategories = (from look in _db.Lookups
                                                 where look.LookupType == "NoticeCategory"
                                                       && look.IsActive == true
                                                       && look.ParentLookupId == not.LookupId
                                                 select new NoticeCategoryModel
                                                 {
                                                     LookupId = look.LookupId,
                                                     LookupName = look.LookupName
                                                 }).ToList()
                         }).OrderBy(x => x.LookupId).ToList();
            return _list != null ? _list : new List<NoticeTypeModel>();
        }

        public List<NoticeTypeModel> GetRecruitmentRuleNoticeHirarchyDetail()
        {
            _db = new upprbDbEntities();
            var _list = (from not in _db.Lookups
                         join parent in _db.Lookups on not.ParentLookupId equals parent.LookupId
                         where not.IsActive == true && not.LookupType == "NoticeType" && parent.LookupType == "UploadType" && parent.LookupName == "RecruitmentRules"
                         select new NoticeTypeModel
                         {
                             LookupId = not.LookupId,
                             LookupName = not.LookupName,
                             NoticeCategories = (from look in _db.Lookups
                                                 where look.LookupType == "NoticeCategory"
                                                       && look.IsActive == true
                                                       && look.ParentLookupId == not.LookupId
                                                 select new NoticeCategoryModel
                                                 {
                                                     LookupId = look.LookupId,
                                                     LookupName = look.LookupName
                                                 }).ToList()
                         }).OrderBy(x => x.LookupId).ToList();
            return _list != null ? _list : new List<NoticeTypeModel>();
        }

        public List<NoticeTypeModel> GetGONoticeHirarchyDetail()
        {
            _db = new upprbDbEntities();
            var _list = (from not in _db.Lookups
                         join parent in _db.Lookups on not.ParentLookupId equals parent.LookupId
                         where not.IsActive == true && not.LookupType == "NoticeType" && parent.LookupType == "UploadType" && parent.LookupName == "GO"
                         select new NoticeTypeModel
                         {
                             LookupId = not.LookupId,
                             LookupName = not.LookupName,
                         }).OrderBy(x => x.LookupId).ToList();
            return _list != null ? _list : new List<NoticeTypeModel>();
        }

        public List<NoticeTypeModel> GetPhotoGalaryNoticeHirarchyDetail()
        {
            _db = new upprbDbEntities();
            var _list = (from not in _db.Lookups
                         join parent in _db.Lookups on not.ParentLookupId equals parent.LookupId
                         where not.IsActive == true && not.LookupType == "NoticeType" && parent.LookupType == "UploadType" && parent.LookupName == "PhotoGalary"
                         select new NoticeTypeModel
                         {
                             LookupId = not.LookupId,
                             LookupName = not.LookupName,
                         }).OrderBy(x => x.LookupId).ToList();
            return _list != null ? _list : new List<NoticeTypeModel>();
        }

        public bool DeleteNotice(int Id)
        {
            _db = new upprbDbEntities();
            var result = _db.Notices.FirstOrDefault(x => x.Id == Id);
            _db.Notices.Remove(result);
            _db.SaveChanges();
            return true;
        }

        public List<EnquiryModel> GetAllEnquiry()
        {
            _db = new upprbDbEntities();
            var _list = (from en in _db.Enquiries
                         select new EnquiryModel
                         {
                             Address = en.Address,
                             Id = en.Id,
                             Email = en.Email,
                             Message = en.Message,
                             Mobile = en.Mobile,
                             Name = en.Name,
                             Subject = en.Subject,
                             CreatedDate = en.CreatedDate
                         }).OrderByDescending(x => x.Id).ToList();
            return _list != null ? _list : new List<EnquiryModel>();
        }

        public List<FeedbackModel> GetAllFeedback()
        {
            _db = new upprbDbEntities();
            var _list = (from en in _db.Feedbacks
                         select new FeedbackModel
                         {
                             Address = en.Address,
                             Id = en.Id,
                             Email = en.Email,
                             Message = en.Message,
                             Mobile = en.Mobile,
                             Name = en.Name,
                             Subject = en.Subject,
                             CreatedDate = en.CreatedDate
                         }).OrderByDescending(x => x.Id).ToList();
            return _list != null ? _list : new List<FeedbackModel>();
        }

        public Enums.CrudStatus SaveEnquiry(Enquiry enquiry)
        {
            _db = new upprbDbEntities();
            _db.Entry(enquiry).State = EntityState.Added;
            int _effectRow = _db.SaveChanges();
            return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }

        public Enums.CrudStatus SaveFeedback(Feedback feedback)
        {
            _db = new upprbDbEntities();
            _db.Entry(feedback).State = EntityState.Added;
            int _effectRow = _db.SaveChanges();
            return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }
        public List<PACEntryModel> GetAllPACDetail()
        {
            _db = new upprbDbEntities();
            var _list = (from pac in _db.PACEntries
                         join state in _db.StateMasters on pac.State_Id equals state.StateId
                         join zone in _db.ZoneMasters on pac.Zone_Id equals zone.ZoneId
                         join range in _db.RangeMasters on pac.Range_Id equals range.RangeId
                         join district in _db.DistrictMasters on pac.District_Id equals district.DistrictId
                         join ps in _db.PSMasters on pac.PS_Id equals ps.PSId
                         select new PACEntryModel
                         {
                             FileUploadName = pac.FileUploadName != null ? pac.FileUploadName : "",
                             CreatedDate = pac.CreatedDate,
                             FileURL = pac.FileURL != null ? pac.FileURL : "",
                             Id = pac.Id,
                             AccusedName = pac.AccusedName,
                             PublishDate = pac.PublishDate,
                             PS_Id = pac.PS_Id,
                             Address = pac.Address,
                             District_Id = pac.District_Id,
                             District_Name = district.DistrictName,
                             ExamineCenterName = pac.ExamineCenterName,
                             FIRDate = pac.FIRDate,
                             FIRDetails = pac.FIRDetails,
                             FIRNo = pac.FIRNo,
                             PS_Name = ps.PSName,
                             Range_Id = pac.Range_Id,
                             Range_Name = range.RangeName,
                             State_Id = pac.State_Id,
                             State_Name = state.StateName,
                             Zone_Id = pac.Zone_Id,
                             Zone_Name = zone.ZoneName
                         }).OrderByDescending(x => x.PublishDate).ToList();
            return _list != null ? _list : new List<PACEntryModel>();
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
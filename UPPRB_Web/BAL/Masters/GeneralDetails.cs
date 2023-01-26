using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using System.Data.Entity;
using UPPRB_Web.Global;
using UPPRB_Web.Models.Masters;
using WebActivatorEx;
using log4net.Repository.Hierarchy;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Web.UI;

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
            var thresoldDate = currentDate.AddMonths(-1);
            var _list = (from not in _db.Notices
                         join look in _db.Lookups on not.EntryTypeId equals look.LookupId
                         where currentDate >= not.NoticeDate && look.LookupType == "UploadType" && look.LookupName == "Notice"
                          && DbFunctions.TruncateTime(thresoldDate) <= DbFunctions.TruncateTime(not.NoticeDate)
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
            var thresoldDate = currentDate.AddMonths(-1);
            var _list = (from not in _db.Notices
                         join look in _db.Lookups on not.EntryTypeId equals look.LookupId
                         where currentDate >= not.NoticeDate && look.LookupType == "UploadType" && look.LookupName == "Notice"
                          && DbFunctions.TruncateTime(thresoldDate) <= DbFunctions.TruncateTime(not.NoticeDate)
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
        public List<PACEntryModel> GetAllPACDetail(int? Id = null)
        {
            _db = new upprbDbEntities();
            var _list = (from pac in _db.PACEntries
                         join state in _db.StateMasters on pac.State_Id equals state.StateId into state1
                         from state2 in state1.DefaultIfEmpty()
                         join zone in _db.ZoneMasters on pac.Zone_Id equals zone.ZoneId into zone1
                         from zone2 in zone1.DefaultIfEmpty()
                         join range in _db.RangeMasters on pac.Range_Id equals range.RangeId into range1
                         from range2 in range1.DefaultIfEmpty()
                         join district in _db.DistrictMasters on pac.District_Id equals district.DistrictId into district1
                         from district2 in district1.DefaultIfEmpty()
                         join ps in _db.PSMasters on pac.PS_Id equals ps.PSId into ps1
                         from ps2 in ps1.DefaultIfEmpty()
                         where (Id == null || (Id != null && pac.Id == Id)) && pac.IsDeleted == false
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
                             District_Name = district2 != null ? district2.DistrictName : "",
                             ExamineCenterName = pac.ExamineCenterName,
                             FIRDate = pac.FIRDate,
                             FIRDetails = pac.FIRDetails,
                             FIRNo = pac.FIRNo,
                             PS_Name = ps2 != null ? ps2.PSName : "",
                             Range_Id = pac.Range_Id,
                             Range_Name = range2 != null ? range2.RangeName : "",
                             State_Id = pac.State_Id,
                             State_Name = state2 != null ? state2.StateName : "",
                             Zone_Id = pac.Zone_Id,
                             Zone_Name = zone2 != null ? zone2.ZoneName : "",
                             PACNumber = pac.PACNumber
                         }).OrderByDescending(x => x.Id).ToList();
            return _list != null ? _list : new List<PACEntryModel>();
        }

        public List<PromotionModel> GetPromotionDetail(int? promotionId = null)
        {
            _db = new upprbDbEntities();
            var _list = (from not in _db.PromotionDetails
                         join parent in _db.PromotionDetails on not.Parent_Id equals parent.Id into parentDetail
                         from parentDetail2 in parentDetail.DefaultIfEmpty()
                         where ((promotionId == null && _db.PromotionDetails.Where(x => x.Parent_Id == null).FirstOrDefault().Id == not.Parent_Id)
                            || (promotionId != null && not.Parent_Id == promotionId))
                         select new PromotionModel
                         {
                             FileName = not.FileName,
                             FIleURL = not.FIleURL,
                             Id = not.Id,
                             Parent_Id = not.Parent_Id,
                             ParentName = parentDetail2 != null ? parentDetail2.Subject : null,
                             Subject = not.Subject,
                             UpdatedDate = not.UpdatedDate.Value
                         }).OrderByDescending(x => x.UpdatedDate).ToList();
            return _list != null ? _list : new List<PromotionModel>();
        }
        public List<DirectRecruitmentModel> GetDirectRecruitmentDetail(int? drId = null)
        {
            _db = new upprbDbEntities();
            var _list = (from not in _db.DirectRecruitementDetails
                         join parent in _db.DirectRecruitementDetails on not.Parent_Id equals parent.Id into parentDetail
                         from parentDetail2 in parentDetail.DefaultIfEmpty()
                         where ((drId == null && _db.DirectRecruitementDetails.Where(x => x.Parent_Id == null).FirstOrDefault().Id == not.Parent_Id)
                            || (drId != null && not.Parent_Id == drId))
                         select new DirectRecruitmentModel
                         {
                             FileName = not.FileName,
                             FIleURL = not.FIleURL,
                             Id = not.Id,
                             Parent_Id = not.Parent_Id,
                             ParentName = parentDetail2 != null ? parentDetail2.Subject : null,
                             Subject = not.Subject,
                             UpdatedDate = not.UpdatedDate.Value
                         }).OrderByDescending(x => x.UpdatedDate).ToList();
            return _list != null ? _list : new List<DirectRecruitmentModel>();
        }
        public IEnumerable<DirectRecruitmentModel> GetRecursiveDirectRecruitmentDetail()
        {
            _db = new upprbDbEntities();
            List<DirectRecruitmentModel> hierarchy = new List<DirectRecruitmentModel>();
            var categories = (from not in _db.DirectRecruitementDetails
                              where (not.FileName == null || not.FileName == "") && (not.FIleURL == null || not.FIleURL == "")
                              select new DirectRecruitmentModel
                              {
                                  FileName = not.FileName,
                                  FIleURL = not.FIleURL,
                                  Id = not.Id,
                                  Parent_Id = not.Parent_Id,
                                  Subject = not.Subject,
                                  UpdatedDate = not.UpdatedDate.Value
                              }).OrderByDescending(x => x.Parent_Id).ToList();

            return categories;

        }
        public IEnumerable<PromotionModel> GetRecursivePromotionDetail()
        {
            _db = new upprbDbEntities();
            List<PromotionModel> hierarchy = new List<PromotionModel>();
            var categories = (from not in _db.PromotionDetails
                              where (not.FileName == null || not.FileName == "") && (not.FIleURL == null || not.FIleURL == "")
                              select new PromotionModel
                              {
                                  FileName = not.FileName,
                                  FIleURL = not.FIleURL,
                                  Id = not.Id,
                                  Parent_Id = not.Parent_Id,
                                  Subject = not.Subject,
                                  UpdatedDate = not.UpdatedDate.Value
                              }).OrderByDescending(x => x.Parent_Id).ToList();

            return categories;

            //hierarchy = categories
            //         .Where(c => c.Parent_Id == null)
            //         .Select(c => new PromotionModel()
            //         {
            //             Id = c.Id,
            //             FileName = c.FileName,
            //             Parent_Id = c.Parent_Id,
            //             FIleURL = c.FIleURL,
            //             Subject = c.Subject,
            //             UpdatedDate = c.UpdatedDate,
            //             Children = GetChildren(categories, c.Id)
            //         })
            //         .ToList();

            //return hierarchy;
        }
        public List<PromotionModel> GetChildren(List<PromotionModel> comments, int parentId)
        {
            return comments
                    .Where(c => c.Parent_Id == parentId)
                    .Select(c => new PromotionModel
                    {
                        Id = c.Id,
                        FileName = c.FileName,
                        Parent_Id = c.Parent_Id,
                        FIleURL = c.FIleURL,
                        Subject = c.Subject,
                        UpdatedDate = c.UpdatedDate,
                        Children = GetChildren(comments, c.Id)
                    })
                    .ToList();
        }

        public Enums.CrudStatus DeletePACEntry(int Id)
        {
            _db = new upprbDbEntities();
            int _effectRow = 0;
            var _deptRow = _db.PACEntries.Where(x => x.Id.Equals(Id)).FirstOrDefault();
            if (_deptRow != null)
            {
                _deptRow.IsDeleted = true;
                //db.PACEntries.Remove(_deptRow);
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
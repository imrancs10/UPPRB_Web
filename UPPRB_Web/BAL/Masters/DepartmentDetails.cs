using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using System.Data.Entity;
using UPPRB_Web.Global;
using UPPRB_Web.Models.Masters;

namespace UPPRB_Web.BAL.Masters
{
    public class DepartmentDetails
    {
        UPPRB_WebEntities _db = null;

        public Enums.CrudStatus SaveDept(string deptName, string deptDesc, string deptUrl)
        {
            _db = new UPPRB_WebEntities();
            int _effectRow = 0;
            var _deptRow = _db.Departments.Where(x => x.DepartmentName.Equals(deptName)).FirstOrDefault();
            var maxDepartmentId = _db.Departments.Max(x => x.DepartmentID);
            if (_deptRow == null)
            {
                Department _newDept = new Department();
                _newDept.DepartmentName = deptName;
                _newDept.DepartmentUrl = deptUrl;
                _newDept.Description = deptDesc;
                _newDept.DepartmentID = maxDepartmentId + 1;
                _db.Entry(_newDept).State = EntityState.Added;
                _effectRow = _db.SaveChanges();
                WebSession.DepartmentId = _newDept.DepartmentID;
                return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
                
            }
            else
                return Enums.CrudStatus.DataAlreadyExist;
        }
        public Enums.CrudStatus EditDept(string deptName, int deptId, string deptUrl,string  deptDesc)
        {
            _db = new UPPRB_WebEntities();
            int _effectRow = 0;
            var _deptRow = _db.Departments.Where(x => x.DepartmentID.Equals(deptId)).FirstOrDefault();
            if (_deptRow != null)
            {
                _deptRow.DepartmentName = deptName;
                _deptRow.DepartmentUrl = deptUrl;
                _deptRow.Description = deptDesc;
                _db.Entry(_deptRow).State = EntityState.Modified;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
            }
            else
                return Enums.CrudStatus.DataNotFound;
        }
        public Enums.CrudStatus UpdateDeptImage(byte[] image, int deptId)
        {
            _db = new UPPRB_WebEntities();
            int _effectRow = 0;
            var _deptRow = _db.Departments.Where(x => x.DepartmentID.Equals(deptId)).FirstOrDefault();
            if (_deptRow != null)
            {
                _deptRow.Image = image;
                _db.Entry(_deptRow).State = EntityState.Modified;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
            }
            else
                return Enums.CrudStatus.DataNotFound;
        }
        public Enums.CrudStatus DeleteDept(int deptId)
        {
            _db = new UPPRB_WebEntities();
            int _effectRow = 0;
            var _deptRow = _db.Departments.Where(x => x.DepartmentID.Equals(deptId)).FirstOrDefault();
            if (_deptRow != null)
            {
                _db.Departments.Remove(_deptRow);
                //_db.Entry(_deptRow).State = EntityState.Deleted;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Deleted : Enums.CrudStatus.NotDeleted;
            }
            else
                return Enums.CrudStatus.DataNotFound;
        }

        public List<DepartmentModel> DepartmentList()
        {
            _db = new UPPRB_WebEntities();
            var _list = (from dept in _db.Departments
                         select new DepartmentModel
                         {
                             DeparmentName = dept.DepartmentName,
                             DepartmentId = dept.DepartmentID,
                             DepartmentUrl = dept.DepartmentUrl,
                             Description = dept.Description,
                             //Image = dept.Image
                             //ImageUrl= string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(dept.Image))
                         }).ToList();
            return _list != null ? _list : new List<DepartmentModel>();
        }
        public List<MasterLookupModel> GetMastersData()
        {
            _db = new UPPRB_WebEntities();
            var _list = (from dept in _db.MasterLookups
                         select new MasterLookupModel
                         {
                             Name = dept.Name,
                             Id = dept.Id,
                             Value = dept.Value
                         }).ToList();
            return _list != null ? _list : new List<MasterLookupModel>();
        }

        public DepartmentModel GetDeparmentById(int deptId)
        {
            _db = new UPPRB_WebEntities();
            int _effectRow = 0;
            var _deptRow = _db.Departments.Where(x => x.DepartmentID.Equals(deptId)).FirstOrDefault();
            if (_deptRow != null)
            {
                var dep = new DepartmentModel()
                {
                    DeparmentName = _deptRow.DepartmentName,
                    DepartmentId = _deptRow.DepartmentID,
                    DepartmentUrl = _deptRow.DepartmentUrl,
                    Description = _deptRow.Description,
                    Image = _deptRow.Image
                };
                return dep;
            }
            return null;
        }

        public Enums.CrudStatus SaveMasterLookup(string name, string value)
        {
            _db = new UPPRB_WebEntities();
            int _effectRow = 0;
            var _deptRow = _db.MasterLookups.Where(x => x.Name.Equals(name)).FirstOrDefault();
            if (_deptRow == null)
            {
                MasterLookup _newDept = new MasterLookup();
                _newDept.Name = name;
                _newDept.Value = value;
                _db.Entry(_newDept).State = EntityState.Added;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
            }
            else
                return Enums.CrudStatus.DataAlreadyExist;
        }

        public Enums.CrudStatus EditMasterLookup(string name, string value, int deptId)
        {
            _db = new UPPRB_WebEntities();
            int _effectRow = 0;
            var _deptRow = _db.MasterLookups.Where(x => x.Id.Equals(deptId)).FirstOrDefault();
            if (_deptRow != null)
            {
                _deptRow.Name = name;
                _deptRow.Value = value;
                _db.Entry(_deptRow).State = EntityState.Modified;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Updated : Enums.CrudStatus.NotUpdated;
            }
            else
                return Enums.CrudStatus.DataNotFound;
        }
        public Enums.CrudStatus DeleteMasterLookup(int deptId)
        {
            _db = new UPPRB_WebEntities();
            int _effectRow = 0;
            var _deptRow = _db.MasterLookups.Where(x => x.Id.Equals(deptId)).FirstOrDefault();
            if (_deptRow != null)
            {
                _db.MasterLookups.Remove(_deptRow);
                //_db.Entry(_deptRow).State = EntityState.Deleted;
                _effectRow = _db.SaveChanges();
                return _effectRow > 0 ? Enums.CrudStatus.Deleted : Enums.CrudStatus.NotDeleted;
            }
            else
                return Enums.CrudStatus.DataNotFound;
        }
    }
}
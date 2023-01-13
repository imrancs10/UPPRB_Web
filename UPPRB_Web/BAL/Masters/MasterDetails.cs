using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPRB_Web.BAL.Masters
{
    public class MasterDetails
    {
        upprbDbEntities _db = null;
        public IEnumerable<object> GetLookupDetail(int? parentLookupId, string lookupTye)
        {
            _db = new upprbDbEntities();
            return (from lookup in _db.Lookups.Where(x => (x.ParentLookupId == parentLookupId) && x.LookupType == lookupTye && x.IsActive == true)
                    select new
                    {
                        lookup.LookupId,
                        lookup.LookupName,
                        lookup.LookupType,
                        lookup.ParentLookupId,
                    }).OrderBy(x => x.LookupName).ToList();

        }
        public IEnumerable<object> GetStateDetail()
        {
            _db = new upprbDbEntities();
            return (from lookup in _db.StateMasters
                    select new
                    {
                        lookup.StateId,
                        lookup.StateName
                    }).OrderBy(x => x.StateName).ToList();

        }

        public IEnumerable<object> GetZoneDetail(int stateId)
        {
            _db = new upprbDbEntities();
            return (from lookup in _db.ZoneMasters
                    where lookup.StateId == stateId
                    select new
                    {
                        lookup.StateId,
                        lookup.ZoneId,
                        lookup.ZoneName,
                    }).OrderBy(x => x.ZoneName).ToList();

        }
        public IEnumerable<object> GetPromotionSubject()
        {
            _db = new upprbDbEntities();
            return (from lookup in _db.PromotionDetails
                    select new
                    {
                        lookup.Subject,
                        lookup.Id,
                        lookup.Parent_Id
                    }).OrderBy(x => x.Parent_Id).ToList();

        }
        public IEnumerable<object> GetDirectRecruitementSubject()
        {
            _db = new upprbDbEntities();
            return (from lookup in _db.DirectRecruitementDetails
                    select new
                    {
                        lookup.Subject,
                        lookup.Id,
                        lookup.Parent_Id
                    }).OrderBy(x => x.Parent_Id).ToList();

        }
        public IEnumerable<object> GetRangeDetail(int zoneId)
        {
            _db = new upprbDbEntities();
            return (from lookup in _db.RangeMasters
                    where lookup.ZoneId == zoneId
                    select new
                    {
                        lookup.RangeId,
                        lookup.ZoneId,
                        lookup.RangeName,
                    }).OrderBy(x => x.RangeName).ToList();

        }

        public IEnumerable<object> GetDistrictDetail(int rangeId)
        {
            _db = new upprbDbEntities();
            return (from lookup in _db.DistrictMasters
                    where rangeId == 0 || lookup.RangeId == rangeId
                    select new
                    {
                        lookup.DistrictId,
                        lookup.RangeId,
                        lookup.DistrictName
                    }).OrderBy(x => x.DistrictName).ToList();
        }
        public IEnumerable<object> GetPoliceStationDetail(int districtId)
        {
            _db = new upprbDbEntities();
            return (from lookup in _db.PSMasters
                    where lookup.DistrictId == districtId
                    select new
                    {
                        lookup.DistrictId,
                        lookup.PSId,
                        lookup.PSName
                    }).OrderBy(x => x.PSName).ToList();
        }

    }
}
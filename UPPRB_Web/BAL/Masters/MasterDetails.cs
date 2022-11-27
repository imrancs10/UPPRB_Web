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
    }
}
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

namespace UPPRB_Web.BAL.Lookup
{
    public class LookupDetails
    {
        upprbDbEntities _db = null;

        enum LookupEnum
        {
            HelpLineNo
        }
        //public List<MasterLookup> GetLookupDetail()
        //{
        //    _db = new upprbDbEntities();
        //    return _db.MasterLookups.ToList();
        //}
    }
}
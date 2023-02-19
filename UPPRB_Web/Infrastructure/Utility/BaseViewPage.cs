using DataLayer;
using UPPRB_Web.Global;
using UPPRB_Web.Infrastructure.Authentication;
using UPPRB_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static UPPRB_Web.Global.Enums;
using UPPRB_Web.BAL.Masters;
using UPPRB_Web.Models.Masters;
using System.Configuration;

namespace UPPRB_Web.Infrastructure.Utility
{
    public abstract class BaseViewPage : WebViewPage
    {
        //public virtual new CustomPrincipal User
        //{
        //    get { return base.User as CustomPrincipal; }
        //}

        //public virtual HospitalDetail GetHospitalDetail()
        //{
        //    HospitalDetails _details = new HospitalDetails();
        //    return _details.GetHospitalDetail();
        //}
        //public virtual int GetAppointmentCount()
        //{
        //    AppointDetails _details = new AppointDetails();
        //    return _details.PatientAppointmentCount(User.Id);
        //}
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual new CustomPrincipal User
        {
            get { return base.User as CustomPrincipal; }
        }
        public virtual List<NoticeModel> GetEntryTypeDetail()
        {
            GeneralDetails _details = new GeneralDetails();
            return _details.GetEntryType();
        }

        public virtual bool GetCaptchEnable()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCaptcha"]);
        }

        //public virtual AppointmentModel GetAppointmentDetail()
        //{
        //    if (User != null)
        //    {
        //        AppointDetails _details = new AppointDetails();
        //        return null;// _details.PatientAppointmentCount(User.Id);
        //    }
        //    return null;
        //}

        //public virtual PatientInfo GetPatientInfo()
        //{
        //    if (User != null)
        //    {
        //        PatientDetails _details = new PatientDetails();
        //        return null;// _details.GetPatientDetailById(User.Id);
        //    }
        //    return null;
        //}

        public virtual PDModel GetPatientOPDDetail()
        {
            //if (User != null)
            //{
            //    string crNumber = string.IsNullOrEmpty(WebSession.PatientCRNo) ? WebSession.PatientRegNo : WebSession.PatientCRNo;
            //    var opdDetail = (new WebServiceIntegration()).GetPatientOPDDetail(crNumber, (Convert.ToInt32(OPDTypeEnum.IPD)).ToString());
            //    return opdDetail;
            //}
            return null;
        }
    }
}
using UPPRB_Web.HISWebReference;
using UPPRB_Web.OPDWebReference;
using UPPRB_Web.Infrastructure.Utility;
using UPPRB_Web.Models;
using UPPRB_Web.PateintInfoService;
using System.IO;
using System.Text;
using log4net;
using System.Collections.Generic;

namespace UPPRB_Web.Infrastructure.Adapter.WebService
{

    public class WebServiceIntegration
    {
        ILog logger = LogManager.GetLogger(typeof(WebServiceIntegration));

        public HISPatientInfoModel GetPatientInfoBYCRNumber(string crNumber)
        {
            GetPatientDetails service = new GetPatientDetails();
            var result = service.getPatientDetails(crNumber);
            if (result.ToUpper().Equals("N") || result.ToUpper().Equals("E"))
                return null;
            Serializer serilizer = new Serializer();
            result = result.Replace("<NewDataSet>", "").Replace("</NewDataSet>", "");
            return serilizer.Deserialize<HISPatientInfoModel>(result, "Table1");
        }

        public string GetPatientInfoinsert(HISPatientInfoInsertModel insertModel)
        {
            //Serializer serilizer = new Serializer();
            string xml = getXMLDataForRegistration(insertModel);
            GetPatient_Info_insert pateintInfoService = new GetPatient_Info_insert();
            return pateintInfoService.GetPatientInfoinsert(xml);
        }

        private string getXMLDataForRegistration(HISPatientInfoInsertModel insertModel)
        {
            string str = "<?xml version=\"1.0\" standalone=\"yes\"?>" +
                        "<NewDataSet>" +
                          "<PatientInfo>" +
                               "<PatientId>" + insertModel.PatientId + "</PatientId>" +
                                "<RegistrationNumber>" + insertModel.RegistrationNumber + "</RegistrationNumber>" +
                                "<MobileNumber>" + insertModel.MobileNumber + "</MobileNumber>" +
                                "<Password>" + insertModel.Password + "</Password>" +
                                "<Email>" + insertModel.Email + "</Email>" +
                                "<Title>" + insertModel.Title + "</Title>" +
                                "<FirstName>" + insertModel.FirstName + "</FirstName>" +
                                "<MiddleName>" + insertModel.MiddleName + "</MiddleName>" +
                                "<LastName>" + insertModel.LastName + "</LastName>" +
                                "<DOB>" + insertModel.DOB + "</DOB>" +
                                "<Gender>" + insertModel.Gender + "</Gender>" +
                                "<MaritalStatus>" + insertModel.MaritalStatus + "</MaritalStatus>" +
                                "<Address>" + insertModel.Address + "</Address>" +
                                "<City>" + insertModel.City + "</City>    " +
                                "<PinCode>" + insertModel.PinCode + "</PinCode>" +
                                "<Religion>" + insertModel.Religion + "</Religion>" +
                                "<DepartmentId>" + insertModel.DepartmentId + "</DepartmentId>  " +
                                "<State>" + insertModel.State + "</State>	" +
                                "<FatherOrHusbandName>" + insertModel.FatherOrHusbandName + "</FatherOrHusbandName>	" +
                                "<Amount>" + insertModel.Amount + "</Amount>" +
                                "<PatientTransactionId>" + insertModel.PatientTransactionId + "</PatientTransactionId>" +
                                "<TransactionNumber>" + insertModel.TransactionNumber + "</TransactionNumber>" +
                                "<CreateDate>" + insertModel.CreateDate + "</CreateDate>	" +
                                "<ValidUpto>" + insertModel.ValidUpto + "</ValidUpto>" +
                                "<Type>" + insertModel.Type + "</Type>" +
                           "</PatientInfo>" +
                        "</NewDataSet>";

            return str;
        }

        public PDModel GetPatientOPDDetail(string crNumber, string type)
        {
            try
            {
                GetPatOpdDetails service = new GetPatOpdDetails();
                var result = service.GetPatientOPDDetails(crNumber, type);
                if (result.ToLower().Contains("no record") || result.ToLower().Contains("N"))
                    return null;
                Serializer serilizer = new Serializer();
                result = result.Replace("<NewDataSet>", "").Replace("</NewDataSet>", "");
                return serilizer.Deserialize<PDModel>(result, "Table1");
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.StackTrace);
            }
            return null;
        }
        public List<DischargeSummaryModel> GetDischargeSummaryDetail(string crNumber, string type)
        {
            try
            {
                GetPatOpdDetails service = new GetPatOpdDetails();
                var result = service.GetPatientOPDDetails(crNumber, type);
                if (result.ToLower().Contains("no record"))
                    return null;
                result = result.Replace("Table1", "dischargeSummaryModel");
                List<DischargeSummaryModel> custList = Serializer.DeserializeDischargeSummary<List<DischargeSummaryModel>>(result);
                return custList;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.StackTrace);
            }
            return null;
        }

        public List<AppointmentsModel> GetMyVisitDetail(string crNumber, string type)
        {
            try
            {
                GetPatOpdDetails service = new GetPatOpdDetails();
                var result = service.GetPatientOPDDetails(crNumber, type);
                if (result.ToLower().Contains("no record"))
                    return null;
                result = result.Replace("Table1", "appointmentsModel");
                List<AppointmentsModel> custList = Serializer.DeserializeDischargeSummary<List<AppointmentsModel>>(result);
                return custList;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.StackTrace);
            }
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DataLayer;
using UPPRB_Web.Global;
using UPPRB_Web.Models.Patient;

namespace UPPRB_Web.BAL.Reports
{
    public class ReportDetails
    {
        upprbDbEntities _db = null;

        public List<PateintLeadger> GetBillReportData()
        {
            _db = new upprbDbEntities();
            var patientInfo = _db.PatientInfoes.Where(x => x.PatientId == WebSession.PatientId).FirstOrDefault();
            var result = _db.PateintLeadgers.Where(x => x.PId == patientInfo.pid).OrderByDescending(x => x.billdate).ToList();
            result.ForEach(x =>
            {
                x.netamt = Math.Round(x.netamt.Value, 2);
                x.godcode = CryptoEngine.Encrypt(Convert.ToString(x.Billid));
                x.vno = CryptoEngine.Encrypt(x.vtype);
                x.salemode = x.billdate.ToString("dd/MM/yyyy");
            });
            return result;
        }

        public Enums.CrudStatus SetBillReportData(int PatientId, string BillNo, string BillType, DateTime BillDate, string ReportUrl, decimal BillAmount, string BillID)
        {
            _db = new upprbDbEntities();
            var patientInfo = _db.PatientInfoes.Where(x => x.PatientId == PatientId).FirstOrDefault();
            PateintLeadger _report = new PateintLeadger();
            _report.netamt = BillAmount;
            _report.billdate = BillDate;
            _report.billno = BillNo;
            _report.Billid = Convert.ToInt32(BillID);
            _report.vtype = BillType;
            //_report.ReportUrl = ReportUrl;
            _report.PId = Convert.ToInt32(patientInfo.pid);
            _db.PateintLeadgers.Add(_report);
            int _result = _db.SaveChanges();
            return _result > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }

        public Enums.CrudStatus SetLabReportData(int PatientId, string BillNo, string RefNo, string ReportUrl, string LabName, DateTime ReportDate, string doctorId)
        {
            _db = new upprbDbEntities();
            var patientInfo = _db.PatientInfoes.Where(x => x.PatientId == PatientId).FirstOrDefault();
            LabreportPdf _report = new LabreportPdf();
            _report.ReportDate = ReportDate;
            _report.Labref = RefNo;
            _report.BillNo = BillNo;
            _report.LabName = LabName;
            _report.CreatedDate = DateTime.Now;
            _report.Url = ReportUrl;
            _report.ModificationDate = DateTime.Now;
            _report.pid = patientInfo.pid;
            _report.DoctorId = Convert.ToInt32(doctorId);
            _db.LabreportPdfs.Add(_report);
            int _result = _db.SaveChanges();
            return _result > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        }

        public List<LabreportPdf> GetLabReportData()
        {
            _db = new upprbDbEntities();
            var patientInfo = _db.PatientInfoes.Where(x => x.PatientId == WebSession.PatientId).FirstOrDefault();
            _db.Configuration.LazyLoadingEnabled = false;
            var result = _db.LabreportPdfs.Where(x => x.pid == patientInfo.pid).OrderBy(x => x.Labref).ToList();
            result.ForEach(x =>
            {
                x.vno = CryptoEngine.Encrypt(x.Url);
                x.Location = x.ReportDate.Value.ToString("dd/MM/yyyy");
            });
            return result;
        }

        public List<PatientTransaction> GetPaymentReceipt()
        {
            _db = new upprbDbEntities();
            _db.Configuration.LazyLoadingEnabled = false;
            var result = _db.PatientTransactions.Where(x => x.PatientId == WebSession.PatientId).OrderBy(x => x.TransactionDate).ToList();
            result.ForEach(x =>
            {
                x.ResponseCode = x.TransactionDate.Value.ToString("dd/MM/yyyy");
                x.StatusCode = x.StatusCode == "S" ? "Success" : "Fail";
            });
            return result;
        }

        public List<PatientLedgerModel> GetPatientLedger(DateTime? fromDate = null, DateTime? toDate = null)
        {
            _db = new upprbDbEntities();
            DateTime _period = DateTime.Now.AddMonths(-WebSession.PatientLedgerPeriodInMonth);
            var patientInfo = _db.PatientInfoes.Where(x => x.PatientId == WebSession.PatientId).FirstOrDefault();
            List<PateintLeadger> data = new List<PateintLeadger>();
            if (fromDate == null && toDate == null)
                data = _db.PateintLeadgers.Where(x => x.PId == patientInfo.pid && DbFunctions.TruncateTime(x.billdate) >= DbFunctions.TruncateTime(_period)).OrderByDescending(x => x.billdate).ToList();
            else
            {
                data = _db.PateintLeadgers.Where(x => x.PId == patientInfo.pid && DbFunctions.TruncateTime(x.billdate) >= DbFunctions.TruncateTime(fromDate) && DbFunctions.TruncateTime(x.billdate) <= DbFunctions.TruncateTime(toDate)).OrderByDescending(x => x.billdate).ToList();
            }
            List<PatientLedgerModel> ledgerList = new List<PatientLedgerModel>();

            if (data != null)
            {
                foreach (var currentLedger in data)
                {
                    PatientLedgerModel newLedger = new PatientLedgerModel();
                    newLedger.Balance = currentLedger.subtotal.ToString();
                    newLedger.Date = currentLedger.billdate == null ? DateTime.Now.ToString("dd/MM/yyyy") : Convert.ToDateTime(currentLedger.billdate).ToString("dd/MM/yyyy");
                    newLedger.Description = getBillType(currentLedger.vtype);
                    newLedger.IPNo = currentLedger.ipno;
                    newLedger.Payment = Math.Round(currentLedger.netamt.Value, 2).ToString();
                    newLedger.Receipt = Math.Round(currentLedger.netamt.Value, 2).ToString();
                    newLedger.Type = currentLedger.vtype;
                    newLedger.VNo = currentLedger.vno;
                    newLedger.schemeid = Convert.ToString(currentLedger.schemeid);
                    newLedger.SaleType = !string.IsNullOrEmpty(currentLedger.saletype) ? currentLedger.saletype.ToUpper() : string.Empty;
                    ledgerList.Add(newLedger);
                }
            }
            return ledgerList;
        }

        private string getBillType(string billtype)
        {
            string desc = string.Empty;
            switch (billtype)
            {
                case "SV":
                    desc = "Procedure/Diagnostic Billing";
                    break;
                case "PH":
                    desc = "Pharmacy Billing-Refund";
                    break;
                case "GP":
                    desc = "Patient Payment";
                    break;
                case "GR":
                    desc = "Receipt from Patient";
                    break;
                case "PHR":
                    desc = "Pharmacy Return";
                    break;
                case "SR":
                    desc = "Sales Return";
                    break;
                case "RS":
                    desc = "Pharmacy Return";
                    break;
                case "SP":
                    desc = "Pharmacy Billing";
                    break;
            }
            return desc;
        }
    }
}
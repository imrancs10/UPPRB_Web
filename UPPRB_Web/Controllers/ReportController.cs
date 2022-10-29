using DataLayer;
using UPPRB_Web.BAL.Reports;
using UPPRB_Web.Global;
using UPPRB_Web.Infrastructure.Adapter.WebService;
using UPPRB_Web.Models;
using UPPRB_Web.Models.Patient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using static UPPRB_Web.Global.Enums;

namespace UPPRB_Web.Controllers
{
    public class ReportController : CommonController
    {
        // GET: Report
        [HttpGet]
        public ActionResult GetBillingReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetBillingReportAsync()
        {
            ReportDetails _details = new ReportDetails();
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            string filterText = Request["search[value]"];
            List<DataLayer.PateintLeadger> result = _details.GetBillReportData();

            if (!string.IsNullOrEmpty(filterText))
            {
                result = result.Where(x => x.billdate.ToString().Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.billno.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.vtype.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.netamt.ToString().Contains(filterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            recordsTotal = result.Count();
            List<DataLayer.PateintLeadger> data = result.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DuplicateBillingReport()
        {
            ReportDetails _details = new ReportDetails();
            List<DataLayer.PateintLeadger> result = _details.GetBillReportData();
            result.ForEach(x =>
            {
                x.netamt = Math.Round(x.netamt.Value, 2);
            });
            return View(result);
        }
        public ActionResult PaymentReciept()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetPaymentReceiptAsync()
        {
            ReportDetails _details = new ReportDetails();
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            string filterText = Request["search[value]"];
            List<PatientTransaction> result = _details.GetPaymentReceipt();

            if (!string.IsNullOrEmpty(filterText))
            {
                result = result.Where(x => x.OrderId.ToString().Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.Amount.ToString().Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.TransactionNumber.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.ResponseCode.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.Type.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.StatusCode.ToString().Contains(filterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            recordsTotal = result.Count();
            List<PatientTransaction> data = result.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult ReportViewing()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReportViewingAsync()
        {
            ReportDetails _details = new ReportDetails();
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            string filterText = Request["search[value]"];
            List<LabreportPdf> result = _details.GetLabReportData();

            if (!string.IsNullOrEmpty(filterText))
            {
                result = result.Where(x => x.Labref.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.LabName.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.BillNo.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.Location.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            recordsTotal = result.Count();
            List<DataLayer.LabreportPdf> data = result.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PatientLedger()
        {
            ReportDetails _details = new ReportDetails();
            return View(_details.GetPatientLedger());
        }

        [HttpPost]
        public ActionResult FilterLeadgerReport(string DateFrom, string DateTo)
        {
            DateTime FromDate = DateTime.Now.AddMonths(-6);
            bool isOKfromdate = DateTime.TryParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);
            if (isOKfromdate)
            {
                FromDate = result;
            }
            DateTime ToDate = DateTime.Now;
            bool isOKtodate = DateTime.TryParseExact(DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultTo);
            if (isOKfromdate)
            {
                ToDate = resultTo;
            }
            int monthsApart = 12 * (FromDate.Year - ToDate.Year) + FromDate.Month - ToDate.Month;
            int diff = Math.Abs(monthsApart);
            if (diff > 6)
            {
                SetAlertMessage("Date Duration should between 6 month", "Leadger Report");
                return RedirectToAction("PatientLedger");
            }
            ReportDetails _details = new ReportDetails();
            List<Models.Patient.PatientLedgerModel> leaders = _details.GetPatientLedger(FromDate, ToDate);
            return View("PatientLedger", leaders);



        }

        public ActionResult DownloadReportFile(string fileUrl)
        {
            string _fileDirectory = fileUrl.Substring(0, fileUrl.LastIndexOf("\\") + 1);
            string _fileName = fileUrl.Substring(fileUrl.LastIndexOf("\\") + 1);
            if (Directory.Exists(_fileDirectory))
            {
                string[] files = Directory.GetFiles(_fileDirectory);
                if (files.Length > 0)
                {
                    string file = files.Where(x => x.Substring(x.LastIndexOf("\\") + 1) == _fileName).FirstOrDefault();
                    if (file != null)
                    {
                        byte[] FileBytes = System.IO.File.ReadAllBytes(file);
                        return File(FileBytes, "application/pdf", _fileName.Substring(0, _fileName.LastIndexOf('.')) + "-" + DateTime.Now.ToShortDateString() + ".pdf");
                    }
                }
            }

            return RedirectToAction("GetBillingReport");
        }

        public ActionResult ViewReportFile(string fileUrl, string _view)
        {
            string _fileDirectory = fileUrl.Substring(0, fileUrl.LastIndexOf("\\") + 1);
            string _fileName = fileUrl.Substring(fileUrl.LastIndexOf("\\") + 1);
            if (Directory.Exists(_fileDirectory))
            {
                string[] _files = Directory.GetFiles(_fileDirectory);
                if (_files.Where(x => x.Substring(fileUrl.LastIndexOf("\\") + 1) == _fileName).Count() > 0)
                {
                    byte[] _fileContent = System.IO.File.ReadAllBytes(fileUrl);
                    return File(_fileContent, "application/pdf");
                }
                return View(_view);
            }
            return RedirectToRoute(fileUrl);
        }

        public ActionResult DownloadFile(string Id)
        {
            string dbURL = CryptoEngine.Decrypt(Convert.ToString(Id)).Replace("~/LabRepPdf", "");
            string url = ConfigurationManager.AppSettings["HISLabReportUrl"] + dbURL;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader responseStream = new StreamReader(response.GetResponseStream());

            MemoryStream ms = new MemoryStream();
            responseStream.BaseStream.CopyTo(ms);

            byte[] imageBytes = ms.ToArray();

            FileContentResult responseFile = new FileContentResult(imageBytes, "application/octet-stream")
            {
                FileDownloadName = "labreport" + DateTime.Now.Date + ".pdf"
            };
            return responseFile;
        }

        public ActionResult ViewBillingReport(string Id, string type)
        {
            string url = ConfigurationManager.AppSettings["HISBillReportUrl"] + "?billid=" + CryptoEngine.Decrypt(Convert.ToString(Id)) + "&vtype=" + CryptoEngine.Decrypt(Convert.ToString(type));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader responseStream = new StreamReader(response.GetResponseStream());

            string resultado = responseStream.ReadToEnd();
            resultado = resultado.Replace("img/rmllogo.jpg",
                                           ConfigurationManager.AppSettings["HISBillReportBaseUrl"] + "/img/rmllogo.jpg");
            return Content(resultado);
        }
        public ActionResult ViewLabReport(string Id)
        {
            string dbURL = CryptoEngine.Decrypt(Convert.ToString(Id)).Replace("~/LabRepPdf", "");
            string url = ConfigurationManager.AppSettings["HISLabReportUrl"] + dbURL;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader responseStream = new StreamReader(response.GetResponseStream());

            MemoryStream ms = new MemoryStream();
            responseStream.BaseStream.CopyTo(ms);

            byte[] imageBytes = ms.ToArray();
            return File(imageBytes, response.ContentType);
        }

        public ActionResult ViewRadiologyLabReport(string Id)
        {
            string url = ConfigurationManager.AppSettings["HISRadiologyReportUrl"] + "?labrefno=" + CryptoEngine.Decrypt(Convert.ToString(Id));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader responseStream = new StreamReader(response.GetResponseStream());

            MemoryStream ms = new MemoryStream();
            responseStream.BaseStream.CopyTo(ms);

            byte[] imageBytes = ms.ToArray();
            return File(imageBytes, response.ContentType);
        }
        [HttpPost]
        public ActionResult PatientLedgerPaymentReport()
        {
            ReportDetails _details = new ReportDetails();
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            List<PatientLedgerModel> ledgerData;
            string filterDateRange = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            string filterText = Request["search[value]"];
            if (!string.IsNullOrEmpty(filterDateRange))
            {
                DateTime FromDate = DateTime.Now.AddMonths(-6);
                bool isOKfromdate = DateTime.TryParseExact(filterDateRange.Split('#')[0], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);
                if (isOKfromdate)
                {
                    FromDate = result;
                }
                DateTime ToDate = DateTime.Now;
                bool isOKtodate = DateTime.TryParseExact(filterDateRange.Split('#')[1], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultTo);
                if (isOKfromdate)
                {
                    ToDate = resultTo;
                }
                ledgerData = _details.GetPatientLedger(FromDate, ToDate).Where(x => x.Type == "GP" || x.Type == "GR").ToList();
            }
            else
            {
                ledgerData = _details.GetPatientLedger().Where(x => x.Type == "GP" || x.Type == "GR").ToList();
            }

            if (!string.IsNullOrEmpty(filterText))
            {
                ledgerData = ledgerData.Where(x => x.Date.ToString().Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.IPNo.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.VNo.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.Receipt.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.Payment.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.Description.ToString().Contains(filterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            ledgerData.ForEach(x =>
            {
                if (x.Type == "GP")
                {
                    x.Receipt = "";
                }
                else
                {
                    x.Payment = "";
                }
            });
            recordsTotal = ledgerData.Count();
            List<PatientLedgerModel> data = ledgerData.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PatientLedgerFormacyReport()
        {
            ReportDetails _details = new ReportDetails();
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            List<PatientLedgerModel> ledgerData;
            string filterDateRange = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            string filterText = Request["search[value]"];
            if (!string.IsNullOrEmpty(filterDateRange))
            {
                DateTime FromDate = DateTime.Now.AddMonths(-6);
                bool isOKfromdate = DateTime.TryParseExact(filterDateRange.Split('#')[0], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);
                if (isOKfromdate)
                {
                    FromDate = result;
                }
                DateTime ToDate = DateTime.Now;
                bool isOKtodate = DateTime.TryParseExact(filterDateRange.Split('#')[1], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultTo);
                if (isOKfromdate)
                {
                    ToDate = resultTo;
                }
                ledgerData = _details.GetPatientLedger(FromDate, ToDate).Where(x => x.Type == "PH" || x.Type == "SV" || x.Type == "SP" || x.Type == "PHR" || x.Type == "SR" || x.Type == "RS").ToList();
            }
            else
            {
                ledgerData = _details.GetPatientLedger().Where(x => x.Type == "PH" || x.Type == "SV" || x.Type == "SP" || x.Type == "PHR" || x.Type == "SR" || x.Type == "RS").ToList();
            }
            ledgerData.ForEach(x =>
            {
                if (x.Type == "PH" || x.Type == "SV" || x.Type == "SP")
                {
                    x.Receipt = "";
                }
                else
                {
                    x.Payment = "";
                }
            });

            if (!string.IsNullOrEmpty(filterText))
            {
                ledgerData = ledgerData.Where(x => x.Date.ToString().Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.IPNo.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.VNo.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.Receipt.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.Payment.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                            || x.Description.ToString().Contains(filterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            recordsTotal = ledgerData.Count();
            List<PatientLedgerModel> data = ledgerData.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DischargeSummary()
        {
            var reports = (new WebServiceIntegration()).GetDischargeSummaryDetail(
                                                    !string.IsNullOrEmpty(WebSession.PatientCRNo) ? WebSession.PatientCRNo : WebSession.PatientRegNo,
                                                    (Convert.ToInt32(OPDTypeEnum.DischargeSummary)).ToString());
            TempData["reports"] = reports;
            return View(reports);
        }
        public ActionResult ViewDischargeSummaryReport(string ipNo)
        {
            var reports = TempData["reports"] as List<DischargeSummaryModel>;
            var report = reports.Where(x => x.ipno == ipNo).FirstOrDefault();
            report.CRNumber = !string.IsNullOrEmpty(WebSession.PatientCRNo) ? WebSession.PatientCRNo : WebSession.PatientRegNo;
            report.Name = WebSession.PatientName;
            report.Gender = WebSession.PatientGender;
            report.Address = WebSession.PatientAddress;
            report.City = WebSession.PatientCity;
            report.MobileNumber = WebSession.PatientMobile;
            return View(report);
        }
    }
}
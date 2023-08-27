using UPPRB_Web.BAL.Commom;
using UPPRB_Web.Global;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;
using Org.BouncyCastle.Asn1.X509;
using DataLayer;
using UPPRB_Web.BAL.Masters;
using UPPRB_Web.Infrastructure.Authentication;
using static iTextSharp.tool.xml.html.HTML;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text;
using UPPRB_Web.Models.Masters;
using System.Security.Principal;
using iTextSharp.tool.xml.html;
using Newtonsoft.Json.Linq;
using UPPRB_Web.Models.Common;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using System.Web.UI.WebControls;
using UPPRB_Web.Infrastructure.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using OfficeOpenXml.Style;

namespace UPPRB_Web.Controllers
{
    [CustomAuthorize]
    public class PACController : CommonController
    {
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult PACDocument()
        {
            return View();
        }
        public ActionResult SearchPACList()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SearchPAC(string ZoneID, string RangeID, string DistrictID, string PSID, string ExamineCenter, string SolverName, string FIRNo, string FIRDateFrom, string FIRDateTo, string RecruitementType, string CenterStatus)
        {
            try
            {
                var detail = new GeneralDetails();
                string draw = Request.Form.GetValues("draw").FirstOrDefault();
                string start = Request.Form.GetValues("start").FirstOrDefault();
                string length = Request.Form.GetValues("length").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                string filterText = Request["search[value]"];
                var result = detail.SearchPACDetail(ZoneID, RangeID, DistrictID, PSID, ExamineCenter, SolverName, FIRNo, FIRDateFrom, FIRDateTo, RecruitementType, CenterStatus);

                recordsTotal = result.Count();
                var data = result.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult GetAllPAC()
        {
            var detail = new GeneralDetails();
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            string filterText = Request["search[value]"];
            var result = detail.GetAllPACDetail();

            if (!string.IsNullOrEmpty(filterText))
            {
                result = result.Where(x => x.AccusedName.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.PS_Name.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.Zone_Name.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.Range_Name.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.FIRNo.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.AccusedName.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.Solver_Name.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.Address.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                          || x.District_Name.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                         || x.RecruitementType.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            recordsTotal = result.Count();
            var data = result.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult CreateExcel(bool IsSearching = false, string ZoneID = "", string RangeID = "", string DistrictID = "", string PSID = "", string ExamineCenter = "", string SolverName = "", string FIRNo = "", string FIRDateFrom = "", string FIRDateTo = "", string RecruitementType = "")
        //{
        //    var detail = new GeneralDetails();
        //    List<PACEntryModel> pacDetailList = new List<PACEntryModel>();
        //    if (IsSearching)
        //        pacDetailList = detail.SearchPACDetail(ZoneID, RangeID, DistrictID, PSID, ExamineCenter, SolverName, FIRNo, FIRDateFrom, FIRDateTo, RecruitementType);
        //    else
        //        pacDetailList = detail.GetAllPACDetail();

        //    try
        //    {

        //        DataTable Dt = Common.ToDataTable(pacDetailList);
        //        var memoryStream = new MemoryStream();
        //        using (var excelPackage = new ExcelPackage(memoryStream))
        //        {
        //            var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
        //            worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
        //            worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
        //            worksheet.DefaultRowHeight = 18;


        //            worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
        //            worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            worksheet.DefaultColWidth = 20;
        //            worksheet.Column(2).AutoFit();

        //            Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
        //            return Json("", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        public void CreateExcel(bool IsSearching = false, string ZoneID = "", string RangeID = "", string DistrictID = "", string PSID = "", string ExamineCenter = "", string SolverName = "", string FIRNo = "", string FIRDateFrom = "", string FIRDateTo = "", string RecruitementType = "", string CenterStatus = "")
        {
            var detail = new GeneralDetails();
            List<PACEntryModel> pacDetailList = new List<PACEntryModel>();
            if (IsSearching)
                pacDetailList = detail.SearchPACDetail(ZoneID, RangeID, DistrictID, PSID, ExamineCenter, SolverName, FIRNo, FIRDateFrom, FIRDateTo, RecruitementType, CenterStatus);
            else
                pacDetailList = detail.GetAllPACDetail();

            var exporData = (from det in pacDetailList
                             select new
                             {
                                 det.RecruitementType,
                                 det.PACNumber,
                                 det.State_Name,
                                 det.Zone_Name,
                                 det.Range_Name,
                                 det.District_Name,
                                 det.PS_Name,
                                 det.CenterStatus,
                                 det.ExamineCenterName,
                                 det.Solver_Name,
                                 det.Address,
                                 det.FIRNo,
                                 det.FIRDate,
                                 det.PublishDate,
                                 det.AccusedName,
                                 det.FIRDetails,
                                 det.CreatedDate
                             }).ToList();

            //ExcelPackage excel = new ExcelPackage();
            string filePath = HttpContext.Server.MapPath("~/Resource/");
            FileInfo template = new FileInfo(filePath + "PAC_Details.xlsx");
            //var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            using (ExcelPackage xlPackage = new ExcelPackage(template))
            {
                ExcelWorksheet workSheet = xlPackage.Workbook.Worksheets["PAC Detail"];
                workSheet.Cells[4, 1].LoadFromCollection(exporData, false);
                
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //here i have set filname as Students.xlsx
                    Response.AddHeader("content-disposition", "attachment;  filename=PAC_Details.xlsx");
                    for (var i = 0; i < exporData.Count; i++)
                    {
                        if (exporData[i].CenterStatus == "Blacklist")
                        {
                            using (ExcelRange Rng = workSheet.Cells[i + 4, 1, i + 4, 17])
                            {
                                System.Drawing.Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#F51B00");
                                Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                Rng.Style.Fill.BackgroundColor.SetColor(colFromHex);
                            }
                        }
                        else if (exporData[i].CenterStatus == "Whitelist")
                        {
                            using (ExcelRange Rng = workSheet.Cells[i + 4, 1, i + 4, 17])
                            {
                                System.Drawing.Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#26D826");
                                Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                Rng.Style.Fill.BackgroundColor.SetColor(colFromHex);
                            }
                        }
                        else if (exporData[i].CenterStatus == "Watchlist")
                        {
                            using (ExcelRange Rng = workSheet.Cells[i + 4, 1, i + 4, 17])
                            {
                                Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                Rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                            }
                        }
                    }
                    xlPackage.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            //workSheet.Cells[1, 1].LoadFromCollection(pacDetailList, true);
            //using (var memoryStream = new MemoryStream())
            //{
            //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    //here i have set filname as Students.xlsx
            //    Response.AddHeader("content-disposition", "attachment;  filename=Students.xlsx");
            //    excel.SaveAs(memoryStream);
            //    memoryStream.WriteTo(Response.OutputStream);
            //    Response.Flush();
            //    Response.End();
            //}
        }
        public FileResult CreatePdf(bool IsSearching = false, string ZoneID = "", string RangeID = "", string DistrictID = "", string PSID = "", string ExamineCenter = "", string SolverName = "", string FIRNo = "", string FIRDateFrom = "", string FIRDateTo = "", string RecruitementType = "", string CenterStatus = "")
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("Preventive Action Cell (CSPAC)" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 12 columns  
            PdfPTable tableLayout = new PdfPTable(14);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table  
            doc.SetPageSize(PageSize.A4.Rotate());
            //file will created in this path  
            string strAttachment = Server.MapPath("~/DownloadFiles/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            string imageURL = Server.MapPath("~/Content/images/logo3.jpg");
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            //Resize image depend upon your need
            jpg.ScaleToFit(280f, 240f);
            //Give space before image
            jpg.SpacingBefore = 10f;
            //Give some space after the image
            jpg.SpacingAfter = 1f;
            jpg.Alignment = Element.ALIGN_CENTER;
            doc.Add(jpg);

            //Add Content to PDF   
            doc.Add(Add_Content_To_PDF(tableLayout, IsSearching, ZoneID, RangeID, DistrictID, PSID, ExamineCenter, SolverName, FIRNo, FIRDateFrom, FIRDateTo, RecruitementType, CenterStatus));

            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            return File(workStream, "application/pdf", strPDFFileName);
        }

        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, bool IsSearching, string ZoneID, string RangeID, string DistrictID, string PSID, string ExamineCenter, string SolverName, string FIRNo, string FIRDateFrom, string FIRDateTo, string RecruitementType, string CenterStatus)
        {
            var detail = new GeneralDetails();
            float[] headers = { 10, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 40 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 95; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  

            List<PACEntryModel> pacDetailList = new List<PACEntryModel>();
            if (IsSearching)
                pacDetailList = detail.SearchPACDetail(ZoneID, RangeID, DistrictID, PSID, ExamineCenter, SolverName, FIRNo, FIRDateFrom, FIRDateTo, RecruitementType, CenterStatus);
            else
                pacDetailList = detail.GetAllPACDetail();

            string pdfTitle = "Cyber Security & Preventive Action Cell (CSPAC) Details";
            if (SolverName == "filterBySolverName")
                pdfTitle += " - Solver Report";
            else if (SolverName == "filterByExamineCenter")
                pdfTitle += " - Blacklisted Report";

            tableLayout.AddCell(new PdfPCell(new Phrase(pdfTitle, new Font(Font.FontFamily.HELVETICA, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 14,
                Border = 0,
                PaddingBottom = 10,
                PaddingLeft = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            ////Add header  
            AddCellToHeader(tableLayout, "Sr No");
            AddCellToHeader(tableLayout, "CSPAC Number");
            //AddCellToHeader(tableLayout, "State");
            AddCellToHeader(tableLayout, "Zone");
            AddCellToHeader(tableLayout, "Range");
            AddCellToHeader(tableLayout, "District");
            AddCellToHeader(tableLayout, "Police Station");
            AddCellToHeader(tableLayout, "Accused Name");
            AddCellToHeader(tableLayout, "Center Status");
            AddCellToHeader(tableLayout, "Center Name");
            AddCellToHeader(tableLayout, "Solver Name");
            AddCellToHeader(tableLayout, "FIR No");
            AddCellToHeader(tableLayout, "FIR Date");
            AddCellToHeader(tableLayout, "Address");
            AddCellToHeader(tableLayout, "FIR Details");

            ////Add body  
            int index = 1;

            var fontPath = Server.MapPath("~/Content/MANGAL.TTF");
            bool hasUnicode;
            foreach (var emp in pacDetailList)
            {

                AddCellToBody(tableLayout, index.ToString());
                AddCellToBody(tableLayout, emp.PACNumber, null, emp.CenterStatus);
                //AddCellToBody(tableLayout, emp.State_Name);
                AddCellToBody(tableLayout, emp.Zone_Name, null, emp.CenterStatus);
                AddCellToBody(tableLayout, emp.Range_Name, null, emp.CenterStatus);
                AddCellToBody(tableLayout, emp.District_Name, null, emp.CenterStatus);
                AddCellToBody(tableLayout, emp.PS_Name, null, emp.CenterStatus);

                AddCellToBody(tableLayout, emp.AccusedName, null, emp.CenterStatus);
                AddCellToBody(tableLayout, emp.CenterStatus, null, emp.CenterStatus);
                AddCellToBody(tableLayout, emp.ExamineCenterName, null, emp.CenterStatus);
                AddCellToBody(tableLayout, emp.Solver_Name, null, emp.CenterStatus);
                AddCellToBody(tableLayout, emp.FIRNo, null, emp.CenterStatus);
                AddCellToBody(tableLayout, emp.FIRDate != null ? emp.FIRDate.Value.ToString("dd/MMM/yyyy") : "", null, emp.CenterStatus);
                AddCellToBody(tableLayout, emp.Address, null, emp.CenterStatus);
                hasUnicode = ContainsUnicodeCharacter(emp.FIRDetails);
                AddCellToBody(tableLayout, emp.FIRDetails, hasUnicode == true ? fontPath : null, emp.CenterStatus);
                index++;
            }

            return tableLayout;
        }
        public bool ContainsUnicodeCharacter(string input)
        {
            const int MaxAnsiCode = 255;

            return input.Any(c => c > MaxAnsiCode);
        }
        // Method to add single cell to the Header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 12, 1, iTextSharp.text.BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(0, 71, 171)
            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText, string fontPath = null, string CenterStatus = "")
        {
            if (fontPath == null)
            {
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 5,
                    BackgroundColor = CenterStatus == "Blacklist" ? new iTextSharp.text.BaseColor(245, 27, 0) : CenterStatus == "Whitelist" ? new iTextSharp.text.BaseColor(38, 216, 38) : CenterStatus == "Watchlist" ? new iTextSharp.text.BaseColor(255, 165, 0) : new iTextSharp.text.BaseColor(255, 255, 255)
                });
            }
            else
            {
                //add hindi font
                BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, false);
                //Set Font and Font Color
                Font font = new Font(bf, 10, Font.NORMAL);

                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, font))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 5,
                    BackgroundColor = CenterStatus == "Blacklist" ? new iTextSharp.text.BaseColor(245, 27, 0) : CenterStatus == "Whitelist" ? new iTextSharp.text.BaseColor(38, 216, 38) : CenterStatus == "Watchlist" ? new iTextSharp.text.BaseColor(255, 165, 0) : new iTextSharp.text.BaseColor(255, 255, 255)
                });
            }
        }
        public ActionResult SearchSolverList()
        {
            return View();
        }

        public ActionResult SearchBlacklistedList()
        {
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("PACLogin", "Home");
        }
    }
}
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
        public JsonResult SearchPAC(string ZoneID, string RangeID, string DistrictID, string PSID, string ExamineCenter, string SolverName, string FIRNo, string FIRDateFrom, string FIRDateTo, string RecruitementType)
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
                var result = detail.SearchPACDetail(ZoneID, RangeID, DistrictID, PSID, ExamineCenter, SolverName, FIRNo, FIRDateFrom, FIRDateTo, RecruitementType);

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

        public FileResult CreatePdf(bool IsSearching = false, string ZoneID = "", string RangeID = "", string DistrictID = "", string PSID = "", string ExamineCenter = "", string SolverName = "", string FIRNo = "", string FIRDateFrom = "", string FIRDateTo = "", string RecruitementType = "")
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("Preventive Action Cell (CSPAC)" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 12 columns  
            PdfPTable tableLayout = new PdfPTable(13);
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
            doc.Add(Add_Content_To_PDF(tableLayout, IsSearching, ZoneID, RangeID, DistrictID, PSID, ExamineCenter, SolverName, FIRNo, FIRDateFrom, FIRDateTo, RecruitementType));

            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            return File(workStream, "application/pdf", strPDFFileName);
        }

        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, bool IsSearching, string ZoneID, string RangeID, string DistrictID, string PSID, string ExamineCenter, string SolverName, string FIRNo, string FIRDateFrom, string FIRDateTo, string RecruitementType)
        {
            var detail = new GeneralDetails();
            float[] headers = { 10, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 40 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 95; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  

            List<PACEntryModel> pacDetailList = new List<PACEntryModel>();
            if (IsSearching)
                pacDetailList = detail.SearchPACDetail(ZoneID, RangeID, DistrictID, PSID, ExamineCenter, SolverName, FIRNo, FIRDateFrom, FIRDateTo, RecruitementType);
            else
                pacDetailList = detail.GetAllPACDetail();

            string pdfTitle = "Cyber Security & Preventive Action Cell (CSPAC) Details";
            if (SolverName == "filterBySolverName")
                pdfTitle += " - Solver Report";
            else if (SolverName == "filterByExamineCenter")
                pdfTitle += " - Blacklisted Report";

            tableLayout.AddCell(new PdfPCell(new Phrase(pdfTitle, new Font(Font.FontFamily.HELVETICA, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 13,
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
            AddCellToHeader(tableLayout, "Examine Center");
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
                AddCellToBody(tableLayout, emp.PACNumber);
                //AddCellToBody(tableLayout, emp.State_Name);
                AddCellToBody(tableLayout, emp.Zone_Name);
                AddCellToBody(tableLayout, emp.Range_Name);
                AddCellToBody(tableLayout, emp.District_Name);
                AddCellToBody(tableLayout, emp.PS_Name);

                AddCellToBody(tableLayout, emp.AccusedName);
                AddCellToBody(tableLayout, emp.ExamineCenterName);
                AddCellToBody(tableLayout, emp.Solver_Name);
                AddCellToBody(tableLayout, emp.FIRNo);
                AddCellToBody(tableLayout, emp.FIRDate != null ? emp.FIRDate.Value.ToString("dd/MMM/yyyy") : "");
                AddCellToBody(tableLayout, emp.Address);
                hasUnicode = ContainsUnicodeCharacter(emp.FIRDetails);
                AddCellToBody(tableLayout, emp.FIRDetails, hasUnicode == true ? fontPath : null);
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
        private static void AddCellToBody(PdfPTable tableLayout, string cellText, string fontPath = null)
        {
            if (fontPath == null)
            {
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10,Font.NORMAL)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 5,
                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
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
                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
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
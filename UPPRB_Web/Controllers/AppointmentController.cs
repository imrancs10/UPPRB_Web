using DataLayer;
using UPPRB_Web.BAL.Appointments;
using UPPRB_Web.BAL.Patient;
using UPPRB_Web.Global;
using UPPRB_Web.Infrastructure;
using UPPRB_Web.Infrastructure.Adapter.WebService;
using UPPRB_Web.Infrastructure.Utility;
using UPPRB_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static UPPRB_Web.Global.Enums;
using JsonResult = System.Web.Mvc.JsonResult;
using System.Data.Entity;
using System.Globalization;

namespace UPPRB_Web.Controllers
{
    public class AppointmentController : CommonController
    {
        // GET: Appointment
        public ActionResult GetAppointments()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DeptWiseDoctorScheduleList(int deptId = 0, int year = 0, int month = 0)
        {
            AppointDetails _details = new AppointDetails();
            return Json(_details.DeptWiseDoctorScheduleList(deptId, year, month), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DayWiseDoctorScheduleList(int deptId, string day, DateTime? date)
        {
            AppointDetails _details = new AppointDetails();
            return Json(_details.DayWiseDoctorScheduleList(deptId, day, date), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DateWiseDoctorAppointmentList(DateTime date)
        {
            AppointDetails _details = new AppointDetails();
            return Json(_details.DateWiseDoctorAppointmentList(date), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveAppointment(AppointmentInfo model, string doctorname, string deptname)
        {
            AppointDetails _details = new AppointDetails();
            string PatientId = Session["PatientId"] == null ? "0" : Session["PatientId"].ToString();
            int pId = 0;
            if (int.TryParse(PatientId, out pId))
            {
                model.PatientId = pId;
                //PatientInfo data = (PatientInfo)Session["PatientData"];
                var user = User;
                Enums.CrudStatus result = _details.SaveAppointment(model);
                if (result == Enums.CrudStatus.Saved)
                {
                    //Send Email
                    Message msg = new Message()
                    {
                        MessageTo = user.Email,
                        MessageNameTo = user.FirstName + " " + user.MiddleName + (string.IsNullOrWhiteSpace(user.MiddleName) ? string.Empty : " ") + user.LastName,
                        Subject = "Appointment Booking Confirmation",
                        Body = EmailHelper.GetAppointmentSuccessEmail(user.FirstName, user.MiddleName, user.LastName, doctorname, model.AppointmentDateFrom, deptname)
                    };
                    ISendMessageStrategy sendMessageStrategy = new SendMessageStrategyForEmail(msg);
                    sendMessageStrategy.SendMessages();

                    //Send SMS
                    msg.Body = "Hello " + string.Format("{0} {1}", user.FirstName, user.LastName) + "\nAs you requested an appointment with " + doctorname + " is  booked on schedule time " + model.AppointmentDateFrom.ToString("dd-MMM-yyyy hh:mm tt") + " at " + deptname + " Department\n Regards:\n Patient Portal(RMLHIMS)";
                    msg.MessageTo = user.Mobile;
                    msg.MessageType = MessageType.Appointment;
                    sendMessageStrategy = new SendMessageStrategyForSMS(msg);
                    sendMessageStrategy.SendMessages();
                    //Infrastructure.SMSService sMSService = new SMSService();
                    //Message smsConfig = new Message()
                    //{
                    //    Body = "Hello " + string.Format("{0} {1}", user.FirstName, user.LastName) + "\nAs you requested an appointment with " + doctorname + " is  booked on schedule time " + model.AppointmentDateFrom.ToString("dd-MMM-yyyy HH:mm tt") + " at " + deptname + " Department\n Regards:\n Patient Portal(RMLHIMS)",
                    //    MessageTo = user.Mobile
                    //};
                    //sMSService.Send(smsConfig);
                }
                return Json(CrudResponse(result), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(CrudResponse(Enums.CrudStatus.SessionExpired), JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(new string[] { "Get", "Post" })]
        public JsonResult GetPatientAppointmentList(int year = 0, int month = 0)
        {
            AppointDetails _details = new AppointDetails();
            int _patientId = 0;
            string _sessionPatienId = Session["PatientId"] == null ? "0" : Session["PatientId"].ToString();
            Dictionary<int, string> result = new Dictionary<int, string>();
            if (int.TryParse(_sessionPatienId, out _patientId))
            {
                return Json(_details.PatientAppointmentList(_patientId, year, month), JsonRequestBehavior.AllowGet);
            }
            else
            {
                result.Add((int)Enums.JsonResult.Invalid_DataId, "Patient Id is invalid");
                return Json(result.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetPatientAppointmentListDataTableFuture()
        {
            AppointDetails _details = new AppointDetails();
            int _patientId = 0;
            string _sessionPatienId = Session["PatientId"] == null ? "0" : Session["PatientId"].ToString();
            if (int.TryParse(_sessionPatienId, out _patientId))
            {
                string draw = Request.Form.GetValues("draw").FirstOrDefault();
                string start = Request.Form.GetValues("start").FirstOrDefault();
                string length = Request.Form.GetValues("length").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                string filterText = Request["search[value]"];
                List<AppointmentsModel> reports = _details.PatientAppointmentList(_patientId, 0, 0)
                                            .Where(x => (x.IsCancelled == false || x.IsCancelled == null)
                                                        && Convert.ToDateTime(x.AppointmentDate) >= DateTime.Now).ToList();
                reports.ForEach(x =>
                {
                    x.AppointmentDate = Convert.ToDateTime(x.AppointmentDate).ToString("dd/MM/yyyy");
                    x.Slot = x.TimeFrom.Substring(0, 5) + " " + x.TimeTo.Substring(0, 5);
                    x.Status = x.IsCancelled != null && x.IsCancelled.Value ? "<strong style='color:red; cursor: pointer'>Cancelled</strong>" : "<strong style='color:green; cursor: pointer'>Booked</strong>";
                });
                if (!string.IsNullOrEmpty(filterText))
                {
                    reports = reports.Where(x => x.DoctorName.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                                || x.Status.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                                || x.AppointmentDate.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                                || x.DepartmentName.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }
                recordsTotal = reports.Count();
                List<AppointmentsModel> data = reports.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                SetAlertMessage("User session is end");
                return RedirectToAction("GetAppointments");
            }
        }

        [HttpPost]
        public ActionResult GetPatientAppointmentListDataTablePast()
        {
            //get data from web service
            var reportFromService = (new WebServiceIntegration()).GetMyVisitDetail(
                                                   !string.IsNullOrEmpty(WebSession.PatientCRNo) ? WebSession.PatientCRNo : WebSession.PatientRegNo,
                                                   (Convert.ToInt32(OPDTypeEnum.MyVisit)).ToString());

            AppointDetails _details = new AppointDetails();
            int _patientId = 0;
            string _sessionPatienId = Session["PatientId"] == null ? "0" : Session["PatientId"].ToString();
            if (int.TryParse(_sessionPatienId, out _patientId))
            {
                string draw = Request.Form.GetValues("draw").FirstOrDefault();
                string start = Request.Form.GetValues("start").FirstOrDefault();
                string length = Request.Form.GetValues("length").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                string filterText = Request["search[value]"];
                List<AppointmentsModel> reports = _details.PatientAppointmentList(_patientId, 0, 0)
                                                          .Where(x => x.IsCancelled == true
                                                                    || Convert.ToDateTime(x.AppointmentDate) < DateTime.Now).ToList();
                reports.ForEach(x =>
                {
                    x.AppointmentDate = Convert.ToDateTime(x.AppointmentDate).ToString("dd/MM/yyyy");
                    x.Status = x.IsCancelled != null && x.IsCancelled.Value
                                    ? "Cancelled on : " + x.CancelDate.Value.ToString("dd/MM/yyyy") + " \n Reason : " + x.CancelReason + "<strong style='color:Red; cursor: pointer'>Cancelled</strong>"
                                    : "<strong style='color:green; cursor: pointer'>Visited</strong>";
                });
                if (reportFromService != null)
                {
                    reportFromService.ForEach(x =>
                    {
                        reports.Add(new AppointmentsModel()
                        {
                            AppointmentDate = Convert.ToDateTime(x.datescheduled).ToString("dd/MM/yyyy"),
                            DepartmentName = x.DepartName,
                            DoctorName = x.DoctorName,
                            fromtime = x.fromtime,
                            totime = x.totime,
                            Status = "<strong style='color:green; cursor: pointer'>Visited</strong>"
                        });
                    });
                }
                if (!string.IsNullOrEmpty(filterText))
                {
                    reports = reports.Where(x => x.DoctorName.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                                || x.Status.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                                || x.AppointmentDate.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)
                                                || x.DepartmentName.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }
                recordsTotal = reports.Count();
                List<AppointmentsModel> data = reports.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                SetAlertMessage("User session is end");
                return RedirectToAction("GetAppointments");
            }
        }

        [HttpPost]
        public JsonResult CancelAppointment(int appointmentId, string CancelReason)
        {
            AppointDetails _details = new AppointDetails();
            int PatientId = 0;
            Dictionary<int, string> result = new Dictionary<int, string>();
            if (int.TryParse(Session["PatientId"].ToString(), out PatientId))
            {
                return Json(_details.CancelAppointment(PatientId, appointmentId, CancelReason).ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                result.Add((int)Enums.JsonResult.Invalid_DataId, "Patient Id is invalid");
                return Json(result.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PatientProfile()
        {
            if (User == null)
            {
                SetAlertMessage("User has been logged out", "Update Profile");
                return RedirectToAction("Index");
            }
            var patient = GetPatientInfo();
            if (patient != null)
            {
                User.FirstName = patient.FirstName;
                User.MiddleName = patient.MiddleName;
                User.LastName = patient.LastName;
                User.Email = patient.Email;
                ViewData["PatientData"] = patient;
            }
            else
            {
                SetAlertMessage("User not found", "Update Profile");
                return RedirectToAction("Index");

            }
            return View();
        }

        public ActionResult CancellationAppointment()
        {
            if (User == null)
            {
                SetAlertMessage("User has been logged out", "Update Profile");
                return RedirectToAction("Index");
            }
            var patient = GetPatientInfo();
            if (patient != null)
            {
                User.FirstName = patient.FirstName;
                User.MiddleName = patient.MiddleName;
                User.LastName = patient.LastName;
                User.Email = patient.Email;
                ViewData["PatientData"] = patient;
            }
            else
            {
                SetAlertMessage("User not found", "Update Profile");
                return RedirectToAction("Index");

            }
            return View();
        }
        private PatientInfoModel GetPatientInfo()
        {
            PatientDetails _details = new PatientDetails();
            var result = _details.GetPatientDetailById(User.Id);
            PatientInfoModel model = new PatientInfoModel
            {
                RegistrationNumber = !string.IsNullOrEmpty(result.CRNumber) ? result.CRNumber : result.RegistrationNumber,
                Address = result.Address,
                CityId = Convert.ToString(result.City),
                Country = result.Country,
                Department = result.Department.DepartmentName,
                DOB = result.DOB,
                Email = result.Email,
                FirstName = result.FirstName,
                Gender = result.Gender,
                LastName = result.LastName,
                MiddleName = result.MiddleName,
                MobileNumber = result.MobileNumber,
                PinCode = result.PinCode,
                Religion = result.Religion,
                StateId = Convert.ToString(result.State),
                Photo = result.Photo
            };
            return model;
        }

    }
}
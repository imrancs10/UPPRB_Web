using DataLayer;
using UPPRB_Web.BAL.Masters;
using UPPRB_Web.BAL.Patient;
using UPPRB_Web.Global;
using UPPRB_Web.Infrastructure;
using UPPRB_Web.Infrastructure.Adapter.WebService;
using UPPRB_Web.Models;
using UPPRB_Web.Models.Masters;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static UPPRB_Web.Global.Enums;
using JsonResult = System.Web.Mvc.JsonResult;

namespace UPPRB_Web.Controllers
{
    public class MastersController : CommonController
    {
        DepartmentDetails _details = null;
        private readonly object FileUpload1;

        public ActionResult AddSchedule()
        {
            return View();
        }

        public ActionResult AddDepartments()
        {
            return View();
        }

        public ActionResult AddDoctors()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveDepartment(string deptName, string deptDesc, string deptUrl)
        {
            _details = new DepartmentDetails();

            return Json(CrudResponse(_details.SaveDept(deptName, deptDesc, deptUrl)), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditDepartment(string deptName, int deptId, string deptDesc, string deptUrl)
        {
            _details = new DepartmentDetails();
            WebSession.DepartmentId = deptId;
            return Json(CrudResponse(_details.EditDept(deptName, deptId, deptUrl, deptDesc)), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDepartment(int deptId)
        {
            _details = new DepartmentDetails();

            return Json(CrudResponse(_details.DeleteDept(deptId)), JsonRequestBehavior.AllowGet);
        }

        public override JsonResult GetDepartments()
        {
            _details = new DepartmentDetails();
            var departments = _details.DepartmentList();
            //departments.ForEach(x =>
            //{
            //    if (x.Image != null)
            //        x.ImageUrl = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(x.Image));
            //});
            return Json(departments, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentImageById(int deptId)
        {
            _details = new DepartmentDetails();
            var department = _details.GetDeparmentById(deptId);
            if (department != null)
            {
                if (department.Image != null)
                    department.ImageUrl = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(department.Image));
            }

            return Json(department, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveDoctor(string doctorName, int deptId, string designation, string degree, string doctorDesc)
        {
            DoctorDetails _details = new DoctorDetails();

            return Json(CrudResponse(_details.SaveDoctor(doctorName, deptId, designation, degree, doctorDesc)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditDoctor(string doctorName, int deptId, int docId, string designation, string degree, string doctorDesc)
        {
            DoctorDetails _details = new DoctorDetails();
            WebSession.DoctorId = docId;
            return Json(CrudResponse(_details.EditDoctor(doctorName, deptId, docId, designation, degree, doctorDesc)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDoctor(int docId)
        {
            DoctorDetails _details = new DoctorDetails();
            return Json(CrudResponse(_details.DeleteDoctor(docId)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDoctors()
        {
            DoctorDetails _details = new DoctorDetails();
            var doctors = _details.DoctorList();
            //doctors.ForEach(x =>
            //{
            //    if (x.Image != null)
            //        x.ImageUrl = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(x.Image));
            //});
            return Json(doctors, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDoctorImageById(int doctorId)
        {
            DoctorDetails _details = new DoctorDetails();
            var doctor = _details.GetDoctorById(doctorId);
            if (doctor != null)
            {
                if (doctor.Image != null)
                    doctor.ImageUrl = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(doctor.Image));
            }

            return Json(doctor, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSchedule(ScheduleModel model)
        {
            ScheduleDetails _details = new ScheduleDetails();
            return Json(CrudResponse(_details.SaveSchedule(model)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditSchedule(ScheduleModel model)
        {
            ScheduleDetails _details = new ScheduleDetails();
            return Json(CrudResponse(_details.EditSchedule(model)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteSchedule(ScheduleModel model)
        {
            ScheduleDetails _details = new ScheduleDetails();

            return Json(CrudResponse(_details.DeleteSchedule(model.ScheduleId)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSchedule()
        {
            ScheduleDetails _details = new ScheduleDetails();
            return Json(_details.ScheduleList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult HospitalDetail()
        {
            HospitalDetails details = new HospitalDetails();
            ViewData["hospitals"] = details.GetAllHospitalDetail();
            return View();
        }

        [HttpPost]
        public ActionResult HospitalDetail(string name, HttpPostedFileBase File1)
        {
            HospitalDetail hospital = new HospitalDetail();
            if (File1 != null && File1.ContentLength > 0)
            {
                hospital.HospitalName = name;
                hospital.HospitalLogo = new byte[File1.ContentLength];
                File1.InputStream.Read(hospital.HospitalLogo, 0, File1.ContentLength);
                HospitalDetails details = new HospitalDetails();
                details.SaveHospital(hospital);
                SetAlertMessage("Hospital detail saved", "Hospital Entry");
                return RedirectToAction("HospitalDetail");
            }
            else
            {
                SetAlertMessage("Hospital detail not saved", "Hospital Entry");
                return RedirectToAction("HospitalDetail");
            }
        }
        public ActionResult DeleteHospital(string Id)
        {
            int result = 0;
            int.TryParse(Id, out result);
            HospitalDetails details = new HospitalDetails();
            details.DeleteHospitalDetail(result);
            return RedirectToAction("HospitalDetail");
        }

        [HttpGet]
        public ActionResult AddDoctorLeave()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDoctorLeaveList(int doctorId)
        {
            DoctorDetails _details = new DoctorDetails();
            return Json(_details.GetDoctorLeaveList(doctorId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveDoctorLeave(int doctorId, DateTime leaveDate)
        {
            DoctorDetails _details = new DoctorDetails();
            return Json(CrudResponse(_details.SaveDoctorLeave(doctorId, leaveDate)), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ViewResult AppointmentSetting()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveAppSetting(AppSettingModel model)
        {
            AppointmentSettingDetails _details = new AppointmentSettingDetails();
            return Json(CrudResponse(_details.SaveAppSetting(model)), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAppSetting()
        {
            AppointmentSettingDetails _details = new AppointmentSettingDetails();
            return Json(_details.GetAppSetting(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LabReport(string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                PatientDetails _detail = new PatientDetails();
                var result = _detail.GetPatientDetailByRegistrationNumberSearch(search);
                ViewData["search"] = search;
                ViewData["PatientInfo"] = result;
            }
            return View();
        }

        public ActionResult SendMessageToPatient()
        {
            PatientDetails _detail = new PatientDetails();
            var result = _detail.GetAllPatientDetail();
            ViewData["PatientInfo"] = result;
            return View();
        }

        [HttpPost]
        public void SendMessageToPatient(string patientIds, string message)
        {
            PatientDetails _detail = new PatientDetails();
            var patientIdList = patientIds.Split(',');
            foreach (var patient in patientIdList)
            {
                var result = _detail.GetPatientDetailById(Convert.ToInt32(patient));
                PatientMessage messageInfo = new PatientMessage()
                {
                    CreatedDate=DateTime.Now,
                    HasRead=false,
                    Message= message,
                    PatientId= result.PatientId,
                };
                _detail.SavePatientMessage(messageInfo);
                SendMessageToPateint(result.FirstName, result.MiddleName, result.LastName, result.MobileNumber, message);
            }
        }

        private async Task SendMessageToPateint(string firstname, string middlename, string lastname, string mobilenumber, string message)
        {
            await Task.Run(() =>
            {
                //Send SMS
                Message msg = new Message()
                {
                    Body = "Hello " + string.Format("{0} {1}", firstname, lastname) + "\n" + message + "\n Regards:\n Patient Portal(RMLHIMS)",
                    MessageTo = mobilenumber,
                    MessageType = MessageType.OTP
                };
                ISendMessageStrategy sendMessageStrategy = new SendMessageStrategyForSMS(msg);
                sendMessageStrategy.SendMessages();
            });
        }

        [HttpPost]
        public ActionResult LabReport(string report, HttpPostedFileBase file, string patientId, string registrationNumber, string searchText)
        {
            PatientDetails patientdetail = new PatientDetails();
            if (file != null && file.ContentLength > 0)
            {
                LabReport labReport = new LabReport
                {
                    CreatedDate = DateTime.Now,
                    FileName = file.FileName,
                    PatientId = int.Parse(patientId),
                    ReportName = report
                };
                bool result = patientdetail.SavePatientLabReport(labReport);
                if (result)
                {
                    string dirUrl = "~/LabReports/" + registrationNumber;
                    string dirPath = Server.MapPath(dirUrl);
                    // Check for Directory, If not exist, then create it  
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    // save the file to the Specifyed folder  
                    string fileUrl = dirUrl + "/" + Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath(fileUrl));
                }
                SetAlertMessage("Lab Report saved", "Hospital Entry");
                return RedirectToAction("LabReport", new { search = searchText });
            }
            else
            {
                SetAlertMessage("Lab Report not saved", "Lab Report");
                return RedirectToAction("LabReport", new { search = searchText });
            }
        }

        public ActionResult SyncHISFailedTransaction()
        {
            PatientDetails _details = new PatientDetails();
            var result = _details.SyncHISFailedPatientList();
            ViewData["PatientInfo"] = result;
            return View();
        }

        [HttpPost]
        public JsonResult SyncHISData(int patientId, int transactionType)
        {
            //send patient data to HIS portal
            PatientDetails _details = new PatientDetails();
            PatientInfo info = _details.GetPatientDetailById(patientId);
            HISPatientInfoInsertModel insertModel = HomeController.setregistrationModelForHISPortal(info);
            insertModel.Type = transactionType;
            WebServiceIntegration service = new WebServiceIntegration();
            string serviceResult = service.GetPatientInfoinsert(insertModel);

            //save status to DB
            PatientInfo user = new PatientInfo()
            {
                PatientId = info.PatientId
            };
            if (insertModel.Type == Convert.ToInt32(TransactionType.Registration))
                user.RegistrationStatusHIS = serviceResult;
            else
                user.RenewalStatusHIS = serviceResult;
            _details.UpdatePatientHISSyncStatus(user);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SyncHISAlreadyData(int patientId)
        {
            PatientDetails _details = new PatientDetails();
            //save status to DB
            PatientInfo user = new PatientInfo()
            {
                PatientId = patientId,
                RenewalStatusHIS = "S",
                RegistrationStatusHIS = "S"
            };
            _details.UpdatePatientHISSyncStatus(user);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddMasterLookup()
        {
            return View();
        }

        public JsonResult GetMastersData()
        {
            _details = new DepartmentDetails();
            return Json(_details.GetMastersData(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveMasterLookup(string name, string value)
        {
            _details = new DepartmentDetails();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                SetAlertMessage("Name or Value should not blank", "Master Data");
                return null;
            }

            return Json(CrudResponse(_details.SaveMasterLookup(name, value)), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditMasterLookup(string name, string value, int deptId)
        {
            _details = new DepartmentDetails();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                SetAlertMessage("LName or Value should not blank", "Lab Report");
                return null;
            }
            return Json(CrudResponse(_details.EditMasterLookup(name, value, deptId)), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteMasterLookup(int deptId)
        {
            _details = new DepartmentDetails();

            return Json(CrudResponse(_details.DeleteMasterLookup(deptId)), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DepartmentImageSave()
        {
            _details = new DepartmentDetails();
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    //Resume
                    HttpPostedFileBase file = files[0];
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    _details.UpdateDeptImage(data, Convert.ToInt32(WebSession.DepartmentId));
                    WebSession.DepartmentId = null;
                    return Json("Image saved.");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        [HttpPost]
        public JsonResult DoctorImageSave()
        {
            DoctorDetails _details = new DoctorDetails();
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    //Resume
                    HttpPostedFileBase file = files[0];
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    _details.UpdateDoctorImage(data, Convert.ToInt32(WebSession.DoctorId));
                    WebSession.DoctorId = null;
                    return Json("Image saved.");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
    }
}
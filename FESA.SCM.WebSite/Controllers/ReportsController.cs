using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using FESA.SCM.WebSite.Helpers;
using FESA.SCM.WebSite.Models;
using FESA.SCM.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Win32;
using System.IO;

namespace FESA.SCM.WebSite.Controllers {
    public class ReportsController : Controller {
        private IAssignmentService _assignmentService;
        private static ResponseFilter dataFilter = new ResponseFilter();
        private ReportModel modelResult;

        public ReportsController(IAssignmentService assignmentService) {
            _assignmentService = assignmentService;
        }
        public IActionResult Index() {
            return View();
        }

        public async Task<IActionResult> ConsultationReports(int page = 1) {
            var result = new ResponseAssignment();
            try {
                var UserSession = HttpContext.Session.Get<UserModel>("UserSesion");
                var dateTemp = DateTime.Today;
                var dateFrom = new DateTime(dateTemp.Year, dateTemp.Month, 1);
                var dateTo = new DateTime(dateTemp.Year, dateTemp.Month + 1, 1).AddDays(-1);
                HttpContext.Session.SetString("StartDateInformFilter", dateFrom.ToString("yyyy-MM-dd"));
                HttpContext.Session.SetString("EndDateInformFilter", dateTo.ToString("yyyy-MM-dd"));
                var pageSize = 10;
                Request request = new Request();

                request = new Request() {
                    Assignment = new AssignmentModel {
                        UserId = UserSession.Id,
                        TypeConsult = 2,
                        WorkOrderNumber = "",
                        CompanyName = "",
                        //StartDate = new DateTime(2017, 1, 20),
                        StartDate = dateFrom,
                        Office = "",
                        CostCenter = "",
                        //EndDate = new DateTime(2018, 1, 20),
                        EndDate = dateTo,
                    },
                    PageIndex = page,
                    PageSize = pageSize
                };

                result = await _assignmentService.GetOtsAsync(request);
                dataFilter = await _assignmentService.GetDataForFilterAsync();
                ViewData["Office"] = AddListItemDataOffices(dataFilter.Offices);
                ViewData["CostCenter"] = AddListItemDataCostsCenter(dataFilter.CostsCenter);
                if (result.Assignments == null) {
                    return View(new ResponseAssignment().Assignments);
                }
                OrderPersonalTechnical(ref result);

                var x = Math.Ceiling(Convert.ToDouble(result.TotalRows / pageSize));
                ViewData["CurrentPage"] = page;
                ViewData["Pages"] = x;
            } catch (Exception) {
                return RedirectToAction("Index", "Home");
            }
            return View(result.Assignments);
        }

        private List<SelectListItem> AddListItemDataOffices(List<OfficeModel> offices) {
            List<SelectListItem> lisItem = new List<SelectListItem>();
            foreach (OfficeModel item in offices) {
                if (!string.IsNullOrEmpty(item.StrOffice.Trim()))
                    lisItem.Add(new SelectListItem { Text = item.StrOffice, Value = item.Id });
            }
            lisItem.Insert(0, new SelectListItem { Text = "Seleccione...", Value = "-1", Selected = true });
            return lisItem;
        }

        private List<SelectListItem> AddListItemDataCostsCenter(List<CostCenterModel> costsCenter) {
            List<SelectListItem> lisItem = new List<SelectListItem>();
            foreach (CostCenterModel item in costsCenter) {
                lisItem.Add(new SelectListItem { Text = item.Description, Value = item.Id });
            }
            lisItem.Insert(0, new SelectListItem { Text = "Seleccione...", Value = "-1", Selected = true });
            return lisItem;
        }

        [HttpPost]
        public IActionResult OTDetailReports(string Id) {
            HttpContext.Session.SetString("IdOTInformes", Id);
            var url = "/ReportsOTDetail";
            return Json(new { Url = url }); ;
        }

        public async Task<IActionResult> ReportsOTDetail() {
            AssignmentModel assig = new AssignmentModel();
            UserModel UserSession = HttpContext.Session.Get<UserModel>("UserSesion");
            try {

                string userId = UserSession.Id;
                int rolId = ConvertUserType.GetStatusUserInt(UserSession.UserType);
                var Id = HttpContext.Session.GetString("IdOTInformes");
                assig = await _assignmentService.GetReportByIdAsync(rolId.ToString(), Id, userId);
                List<ContactModel> FerreContacts = new List<ContactModel>();
                FerreContacts.AddRange(GetChargeInFerreyrosContact("Supervisor", assig.FerreyrosContacts));
                FerreContacts.AddRange(GetChargeInFerreyrosContact("Líder", assig.FerreyrosContacts));
                FerreContacts.AddRange(GetChargeInFerreyrosContact("Técnico", assig.FerreyrosContacts));
                assig.FerreyrosContacts = FerreContacts;
                CalculateDelayedHours(ref assig);
                if (assig.Report.Files != null)
                    AddStatusFile(ref assig);
                HttpContext.Session.SetString("TitleForm", "Consulta de Informes");
                HttpContext.Session.SetString("ButtonBack", "BackToReports");
            } catch (Exception) {

                return RedirectToAction("ConsultationReports", "Reports");

            }
            HttpContext.Session.Set<AssignmentModel>("ReporModelSupervisor", assig);
            return View("ReportsOT", assig);
        }

        private void CalculateDelayedHours(ref AssignmentModel assig) {
            TimeSpan StandarTime1 = TimeSpan.FromMinutes(Convert.ToInt16(assig.Report.TotalMinuteStandard1));
            TimeSpan StandarTime2 = TimeSpan.FromMinutes(Convert.ToInt16(assig.Report.TotalMinuteStandard2));
            assig.Report.DeliveryTime1 = assig.Report?.Date1 - assig.Report?.Date;
            assig.Report.DeliveryTime2 = assig.Report?.Date2 - assig.Report?.Date;
            assig.Report.DeliveryTime1 = assig.Report?.Date1 - assig.Report?.Date;
            assig.Report.DeliveryTime2 = assig.Report?.Date2 - assig.Report?.Date;
            assig.Report.DelayedTime1 = assig.Report.DeliveryTime1 - StandarTime1;
            assig.Report.DelayedTime2 = assig.Report.DeliveryTime2 - StandarTime2;

        }

        private void AddStatusFile(ref AssignmentModel assig) {
            List<FileModel> newListModel = new List<FileModel>();
            foreach (var item in assig.Report.Files) {
                FileModel fileNew = new FileModel();
                fileNew = item;
                fileNew.Status = 2;
                newListModel.Add(fileNew);
            }
            assig.Report.Files = newListModel;
        }

        public IActionResult ReportsDetail() {
            var AssignmentDetailTechnical = HttpContext.Session.Get<AssignmentModel>("DetailTechnical");
            return View("ReportsOT", AssignmentDetailTechnical);
        }

        private List<ContactModel> GetChargeInFerreyrosContact(string charge, List<ContactModel> FerreyrosContacts) {
            List<ContactModel> Result = new List<ContactModel>();
            foreach (ContactModel item in FerreyrosContacts) {
                if (string.Equals(item.Charge, charge)) {
                    Result.Add(item);
                }
            }
            return Result;
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchModel model) {
            ResponseAssignment result = new ResponseAssignment();
            DateTime parseStart = new DateTime(1900, 1, 1);
            DateTime parseEnd = new DateTime(2900, 1, 1); ;
            try {
                var UserSession = HttpContext.Session.Get<UserModel>("UserSesion");
                if (!string.IsNullOrEmpty(model.StartDate)) {
                    DateTime.TryParse(model.StartDate, out parseStart);
                }
                if (!string.IsNullOrEmpty(model.EndDate)) {
                    DateTime.TryParse(model.EndDate, out parseEnd);
                }
                Request request = new Request() {
                    Assignment = new AssignmentModel {
                        UserId = UserSession.Id,
                        TypeConsult = 2,
                        WorkOrderNumber = string.IsNullOrEmpty(model.WorkOrderNumber) ? string.Empty : model.WorkOrderNumber.Trim(),
                        CompanyName = string.IsNullOrEmpty(model.Client) ? string.Empty : model.Client.Trim(),
                        StartDate = parseStart,
                        Office = string.IsNullOrEmpty(model.IdOffice) || model.IdOffice == "-1" ? string.Empty : model.IdOffice,
                        CostCenter = string.IsNullOrEmpty(model.IdCostCenter) || model.IdOffice == "-1" ? string.Empty : model.IdCostCenter,
                        EndDate = parseEnd,
                        Supervisor = string.IsNullOrEmpty(model.Supervisor) ? string.Empty : model.Supervisor.Trim(),
                        Brand = string.IsNullOrEmpty(model.IdBrand) || model.IdBrand == "-1" ? string.Empty : model.IdBrand,
                    },
                    PageIndex = 1,
                    PageSize = 10
                };
                result = await _assignmentService.GetOtsAsync(request);
                OrderPersonalTechnical(ref result);
                KeepFilterData(model);
                ViewData["Office"] = AddListItemDataOffices(dataFilter.Offices);
                ViewData["CostCenter"] = AddListItemDataCostsCenter(dataFilter.CostsCenter);
                if (result.TotalRows == 0) {
                    return PartialView("_ConsultationReportList", new List<AssignmentModel>());
                }

            } catch (Exception e) {
                return PartialView("_ConsultationReportList", new List<AssignmentModel>());
            }
            return PartialView("_ConsultationReportList", result.Assignments.OrderBy(i => i.EndDate).ToList());
        }

        private void KeepFilterData(SearchModel model) {
            HttpContext.Session.SetString("OTInformFilter", string.IsNullOrEmpty(model.WorkOrderNumber) ? string.Empty : model.WorkOrderNumber.Trim());
            HttpContext.Session.SetString("ClientInformFilter", string.IsNullOrEmpty(model.Client) ? string.Empty : model.Client.Trim());
            HttpContext.Session.SetString("StartDateInformFilter", string.IsNullOrEmpty(model.StartDate) ? HttpContext.Session.GetString("StartDateInformFilter") : model.StartDate.Trim());
            HttpContext.Session.SetString("EndDateInformFilter", string.IsNullOrEmpty(model.EndDate) ? HttpContext.Session.GetString("EndDateInformFilter") : model.EndDate.Trim());
        }

        private void OrderPersonalTechnical(ref ResponseAssignment list) {
            try {
                List<AssignmentModel> listNewAssignment = new List<AssignmentModel>();
                foreach (var item in list.Assignments) {
                    AssignmentModel newModel = new AssignmentModel();
                    List<ContactModel> FerreContacts = new List<ContactModel>();
                    FerreContacts.AddRange(GetChargeInFerreyrosContact("Líder", item.FerreyrosContacts));
                    FerreContacts.AddRange(GetChargeInFerreyrosContact("Técnico", item.FerreyrosContacts));
                    FerreContacts.AddRange(GetChargeInFerreyrosContact("Supervisor", item.FerreyrosContacts));
                    newModel = item;
                    newModel.FerreyrosContacts = FerreContacts;
                    listNewAssignment.Add(newModel);
                }
                list.Assignments = listNewAssignment;
            } catch (Exception) {

            }
        }

        public IActionResult BackToReports() {
            return RedirectToAction("ConsultationReports");
        }

        public IActionResult BackToOT() {
            return RedirectToAction("WorkOrder", "OT");
        }


        [HttpPost]
        public async Task<IActionResult> SaveInform([FromBody]ReportModel model) {
            string status = "1";
            try {
                UserModel UserSession = HttpContext.Session.Get<UserModel>("UserSesion");
                model.ModifiedBy = UserSession.Name;
                ClientResponse clientResponse = await _assignmentService.UpdateExecutiveInform(model);
                if (clientResponse == null || clientResponse.Status != System.Net.HttpStatusCode.OK) {
                    status = "0";
                    return Json(new { content = "Error", Status = status });
                }
            } catch (Exception) {
                return Json(new { content = "Error" });
            }

            return Json(new { content = "Ok", Status = status });
        }

        [HttpPost]
        public async Task<IActionResult> UploadTechnicalFile() {
            ReportModel model = new ReportModel();
            model.Contact = new ContactModel();
            model.Files = new List<FileModel>();
            var Assign = HttpContext.Session.Get<AssignmentModel>("ReporModelSupervisor");
            try {
                UserModel UserSession = HttpContext.Session.Get<UserModel>("UserSesion");
                model.Id = Assign.Report.Id;
                model.AssignmentId = Request.Form["AssignmentId"];
                model.TotalMinute = Convert.ToInt16(Request.Form["TotalMinute"]);
                model.ModifiedBy = UserSession.Name;
                model.ActionType = Convert.ToInt16(Request.Form["action"]);
                model.Comment1 = Request.Form["Comment1"];
                model.NameFile = "1";
                model.UrlFile = Assign.Report.UrlFile;
                ClientResponse clientResponse = null;
                if (Request.Form.Keys.Count > 0) {
                    if (Request.Form.Files.Count > 0) {
                        IFormFile file = Request.Form.Files[0];
                        byte[] data;
                        using (var br = new BinaryReader(file.OpenReadStream()))
                            data = br.ReadBytes((int)file.OpenReadStream().Length);

                        if (!string.IsNullOrEmpty(Request.Form["Id"]))
                            model.Id = Request.Form["Id"];

                        model.NameFile = file.FileName.Trim();
                        model.FileData = data;

                    } 
                      
                    


                }
                clientResponse = await _assignmentService.UpdateArchiveInform(model);
                if (clientResponse == null || clientResponse.Status != System.Net.HttpStatusCode.OK) {
                    return Json(new { content = "Error" });
                }
            } catch (Exception) {
                return Json(new { content = "Error" });
            }
            return Json(new { content = "Ok" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteArchiveInformFinal([FromBody]ReportModel model) {
            string status = "1";
            try {
                UserModel UserSession = HttpContext.Session.Get<UserModel>("UserSesion");
                model.ModifiedBy = UserSession.Name;
                ClientResponse clientResponse = await _assignmentService.DeleteArchiveInform(model);
                if (clientResponse == null || clientResponse.Status != System.Net.HttpStatusCode.OK) {
                    status = "0";
                    return Json(new { content = "Error", Status = status });
                }
            } catch (Exception) {
                return Json(new { content = "Error" });
            }

            return Json(new { content = "Ok", Status = status });
        }


        [HttpPost]
        public async Task<IActionResult> UploadFileSupervisor() {
            var model = new ReportModel();

            var UserSession = HttpContext.Session.Get<UserModel>("UserSesion");
            var Assign = HttpContext.Session.Get<AssignmentModel>("ReporModelSupervisor");
            model = Assign.Report;
            modelResult = Assign.Report;

            try {
                if (!Request.HasFormContentType) {
                    return Json(new { content = "Error" });
                }
                foreach (var file in Request.Form.Files) {
                    byte[] dataSupervisor;
                    using (var br = new BinaryReader(file.OpenReadStream()))
                        dataSupervisor = br.ReadBytes((int)file.OpenReadStream().Length);
                    model.Files = new List<FileModel>();
                    string[] nameArr = file.FileName.Trim().Split(".");
                    var fileModel = new FileModel();

                    fileModel.Id = Guid.NewGuid().ToString();
                    fileModel.IdRef = model.Id;
                    fileModel.Name = file.FileName.Trim().Replace(" ", "");
                    fileModel.Ext = nameArr[1];
                    fileModel.FileData = dataSupervisor;
                    fileModel.CreatedBy = UserSession.Name;
                    model.Files.Add(fileModel);

                }
                ClientResponse clientResponse = null;
                clientResponse = await _assignmentService.AddFileToReportSupervisor(model);
                modelResult.Files.AddRange(model.Files);
                if (clientResponse == null || clientResponse.Status != System.Net.HttpStatusCode.OK) {
                    return Json(new { content = "Error" });
                }
            } catch (Exception) {
            }
            return PartialView("_FileListSupervisor", modelResult);
        }

        [HttpPost]
        public IActionResult DeleteFileFinalInformSupervisor([FromBody]FileModel model) {
            ReportModel modelResult = new ReportModel();
            try {
                AssignmentModel Asign = HttpContext.Session.Get<AssignmentModel>("ReporModelSupervisor");
                UserModel UserSession = HttpContext.Session.Get<UserModel>("UserSesion");
                modelResult = Asign.Report;
                List<FileModel> listFiles = new List<FileModel>();
                foreach (var item in modelResult.Files) {
                    FileModel newFile = new FileModel();
                    newFile = item;
                    newFile.ModifiedBy = UserSession.Name;
                    //if (item.Status != 0) {
                        if (item.Id == model.Id)
                            newFile.Status = 1;
                        listFiles.Insert(0, newFile);
                    //}

                }
                modelResult.Files = listFiles;
                Asign.Report = modelResult;
                HttpContext.Session.Remove("ReporModelSupervisor");
                HttpContext.Session.Set<AssignmentModel>("ReporModelSupervisor", Asign);


            } catch (Exception) {

            }
            return PartialView("_FileListSupervisor", modelResult);
        }

        [HttpPost]
        public IActionResult AddSupervisorFile() {
            UserModel UserSession = HttpContext.Session.Get<UserModel>("UserSesion");
            AssignmentModel Asign = HttpContext.Session.Get<AssignmentModel>("ReporModelSupervisor");
            ReportModel ReportModelResult = Asign.Report;
            try {
                if (!Request.HasFormContentType) {
                    return Json(new { content = "Error" });
                }
                if (Request.Form.Files.Count > 0) {
                    if (Request.Form.Files["FilesSupervisorAdd"] != null) {
                        IFormFile fileSuper = Request.Form.Files["FilesSupervisorAdd"];
                        byte[] dataSupervisor;
                        using (var br = new BinaryReader(fileSuper.OpenReadStream()))
                            dataSupervisor = br.ReadBytes((int)fileSuper.OpenReadStream().Length);

                        string[] nameArr = fileSuper.FileName.Trim().Split(".");
                        FileModel fModel = new FileModel();
                        fModel.Id = Guid.NewGuid().ToString();
                        fModel.IdRef = ReportModelResult.Id;
                        fModel.Name = fileSuper.FileName.Trim().Replace(" ", "");
                        fModel.Ext = nameArr[1];
                        fModel.FileData = dataSupervisor;
                        fModel.CreatedBy = UserSession.Name;
                        fModel.Status = 0;
                        if (ReportModelResult.Files == null)
                            ReportModelResult.Files = new List<FileModel>();
                        ReportModelResult.Files.Insert(0, fModel);
                    }
                }
            } catch (Exception) {
            }
            Asign.Report = ReportModelResult;
            HttpContext.Session.Remove("ReporModelSupervisor");
            HttpContext.Session.Set<AssignmentModel>("ReporModelSupervisor", Asign);
            return PartialView("_FileListSupervisor", ReportModelResult);
        }

        [HttpPost]
        public async Task<IActionResult> SaveInfoSupervisor() {
            UserModel userSession = HttpContext.Session.Get<UserModel>("UserSesion");
            AssignmentModel asign = HttpContext.Session.Get<AssignmentModel>("ReporModelSupervisor");
            ReportModel reportModelResult = null;
            if (asign.Report == null) {
                reportModelResult = new ReportModel();
            } else {
                reportModelResult = asign.Report;
            }
            reportModelResult.NameFile = string.Empty;
            reportModelResult.FileData = new byte[] { };
            try {
                if (!Request.HasFormContentType) {
                    return Json(new { content = "Error" });
                }
                if (Request.Form.Files.Count > 0) {
                    if (Request.Form.Files["FileTechnical"] != null) {
                        IFormFile file = Request.Form.Files["FileTechnical"];
                        byte[] data;
                        using (var br = new BinaryReader(file.OpenReadStream()))
                            data = br.ReadBytes((int)file.OpenReadStream().Length);

                        reportModelResult.AssignmentId = asign.Id;
                        reportModelResult.FileData = data;
                        reportModelResult.ModifiedBy = userSession.Name;
                        reportModelResult.NameFile = file.FileName.Trim();
                    }
                }
                List<FileModel> listFiles = new List<FileModel>();
                foreach (var item in reportModelResult.Files) {
                    FileModel newFile = new FileModel();
                    newFile = item;
                    if (item.Status != 2) {
                        if (item.Status == 1) {
                            newFile.FileData = new byte[] { };
                            newFile.Name = string.Empty;
                        }
                        listFiles.Insert(0, newFile);
                    }
                }
                reportModelResult.AssignmentId = asign.Id;
                reportModelResult.Comment2 = Request.Form["Comment2"];
                string strCheck = Request.Form["Check"];
                reportModelResult.ActionType = Convert.ToInt16(Request.Form["action"]);
                if (strCheck == "on") {
                    reportModelResult.Check = true;
                }
                if (reportModelResult.ActionType == 2) {
                    reportModelResult.Sent2 = true;
                }
                reportModelResult.Files = listFiles;
                ClientResponse clientResponse = await _assignmentService.AddOrUpdateSendFileToReportSupervisor(reportModelResult);
                if (clientResponse == null || clientResponse.Status != System.Net.HttpStatusCode.OK) {
                    return Json(new { content = "Error" });
                }
            } catch (Exception) {
                return Json(new { content = "Error" });
            }
            return Json(new { content = "Ok" });
        }
    }
}
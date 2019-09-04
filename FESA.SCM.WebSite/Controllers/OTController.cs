using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using FESA.SCM.WebSite.Helpers;
using FESA.SCM.WebSite.Models;
using FESA.SCM.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FESA.SCM.WebSite.Controllers {
    //[Authorize]
    public class OTController : Controller {
        private IAssignmentService _assignmentService;
        private static ResponseFilter dataFilter = new ResponseFilter();
        public OTController(IAssignmentService assignmentService) {
            _assignmentService = assignmentService;
        }

        public IActionResult Index() {
            return View();
        }

        public async Task<IActionResult> WorkOrder(int page = 1) {
            var result = new ResponseAssignment();
            try {
               
                var UserSession = HttpContext.Session.Get<UserModel>("UserSesion");
                if (UserSession.UserType == UserType.Technician) {
                    throw new Exception();
                }
                    var dateTemp = DateTime.Today;
                    var dateFrom = new DateTime(dateTemp.Year, dateTemp.Month, 1);
                    var dateTo = new DateTime(dateTemp.Year, dateTemp.Month + 1, 1).AddDays(-1);
                    HttpContext.Session.SetString("StartDateFilter", dateFrom.ToString("yyyy-MM-dd"));
                    HttpContext.Session.SetString("EndDateFilter", dateTo.ToString("yyyy-MM-dd"));

                    var pageSize = 10;
                    Request request = new Request();

                    request = new Request() {
                        Assignment = new AssignmentModel {
                            UserId = UserSession.Id,
                            TypeConsult = 1,
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

                    OrderPersonalTechnical(ref result);
                    ViewData["Office"] = AddListItemDataOffices(dataFilter.Offices);
                    ViewData["CostCenter"] = AddListItemDataCostsCenter(dataFilter.CostsCenter);
                    ViewData["Brand"] = AddListItemDataBrands(dataFilter.Brands);
                    var x = Math.Ceiling(Convert.ToDouble(result.TotalRows / pageSize));
                    ViewData["CurrentPage"] = page;
                    ViewData["Pages"] = x;
            } catch (Exception) {
                return RedirectToAction("Index", "Home");
            }
            return View(result.Assignments);
        }

        [HttpPost]
        public IActionResult DetailOT(string Id) {
            HttpContext.Session.SetString("IdOT", Id);
            var url = "/OTDetail";
            return Json(new { Url = url }); ;
        }
        public async Task<IActionResult> OTDetail() {
            try {
                var UserSession = HttpContext.Session.Get<UserModel>("UserSesion");
                string userId = UserSession.Id;
                var Id = HttpContext.Session.GetString("IdOT");
                AssignmentModel Assig = new AssignmentModel();
                Assig = await _assignmentService.GetOtAsync(Id, userId);
                List<ContactModel> FerreContacts = new List<ContactModel>();
                FerreContacts.AddRange(GetChargeInFerreyrosContact("Supervisor", Assig.FerreyrosContacts));
                FerreContacts.AddRange(GetChargeInFerreyrosContact("Líder", Assig.FerreyrosContacts));
                FerreContacts.AddRange(GetChargeInFerreyrosContact("Técnico", Assig.FerreyrosContacts));
                Assig.FerreyrosContacts = FerreContacts;
                HttpContext.Session.Set<List<ContactModel>>("ListContact", Assig.TechnicalContacts);
                return View("DetailOT", Assig);
            } catch (Exception) {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult OTDetailTechnical(string Id, string IdTechnical ) {
            try {
                HttpContext.Session.SetString("IdOT", Id);
                HttpContext.Session.SetString("IdTehcnicalDetail", IdTechnical);
                return Json(new { content = "Ok" });
            } catch (Exception) {
            }
            return Json(new { content = "Error" });
        }

        public async Task<IActionResult> TechnicalDetail() {
            var UserSession = HttpContext.Session.Get<UserModel>("UserSesion");
            var IdTechnical = HttpContext.Session.GetString("IdTehcnicalDetail");
            string userId = IdTechnical;
            var Id = HttpContext.Session.GetString("IdOT");
            HttpContext.Session.SetString("IdOT", Id);
            HttpContext.Session.SetString("IdTehcnicalDetail", IdTechnical);
            string Tecnichal = string.Empty;
            ContactModel TechnicalContact = new ContactModel();

            AssignmentModel Assign = new AssignmentModel();
            ResponseAssignment ResponseDetailDocActiv = new ResponseAssignment();
            try {
                Assign = await _assignmentService.GetOtAsync(Id, userId);
                ResponseDetailDocActiv = await _assignmentService.GetOtDetailAsync(Id, userId);
                Assign.Activities = ResponseDetailDocActiv.Assignment.Activities.OrderByDescending(k => k.StartDate).ToList();
                CalculateActivityDuration(ref Assign);
                TechnicalContact = Assign.FerreyrosContacts.Where(i => i.Id == IdTechnical).FirstOrDefault();
                ViewBag.DocumentsEntity = ResponseDetailDocActiv.DocumentsEntity;
                ViewBag.TotalDocumentsOT = 1;
                if (ResponseDetailDocActiv.Assignment.Documents == null || ResponseDetailDocActiv.Assignment.Documents.Count == 0) {
                    //ViewBag.TotalDocumentsOT = 0;
                    ViewBag.TotalDocumentsOT = 0;
                } else {
                    ViewBag.DocumentsAssignment = ResponseDetailDocActiv.Assignment.Documents.OrderBy(i => i.Position).ToList();
                }
                ViewBag.TechnicalName = TechnicalContact.Name + " " + TechnicalContact.LastName;
                ViewBag.TecnihalStatus = ConvertContactStatus.GetStatusSPA(TechnicalContact.assignmentStatus);
                ViewBag.Charge = TechnicalContact.Charge;
            } catch (Exception) {
                return RedirectToAction("WorkOrder", "OT");
            }
            return View(Assign);
        }



        [HttpPost]
        public async Task<IActionResult> SaveContact(ContactModel newContact) {
            newContact.CustomerId = newContact.IdCustomer;
            newContact.Name = newContact.Name.ToUpper();
            newContact.LastName = newContact.LastName.ToUpper();
            newContact.Email = newContact.Email.ToUpper();
            List<ContactModel> ListContacts = HttpContext.Session.Get<List<ContactModel>>("ListContact");
            HttpContext.Session.Remove("ListContact");
            newContact.assignmentStatus = ContactStatus.Assigned;
            string contactResponseId = await _assignmentService.SaveContact(newContact);
            newContact.Id = contactResponseId;
            ListContacts.Add(newContact);
            return PartialView("_DetailTechnicalContacts", ListContacts);
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

        private List<SelectListItem> AddListItemDataOffices(List<OfficeModel> offices) {
            List<SelectListItem> lisItem = new List<SelectListItem>();
            foreach (OfficeModel item in offices) {
                if(!string.IsNullOrEmpty(item.StrOffice.Trim()))
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

        private List<SelectListItem> AddListItemDataBrands(List<BrandModel> brands) {
            List<SelectListItem> lisItem = new List<SelectListItem>();
            foreach (BrandModel item in brands) {
                lisItem.Add(new SelectListItem { Text = item.Name, Value = item.Id });
            }
            lisItem.Insert(0, new SelectListItem { Text = "Seleccione...", Value = "-1", Selected = true });
            return lisItem;
        }


        [HttpPost]
        public async Task<IActionResult> Search(SearchModel model) {
            var UserSession = HttpContext.Session.Get<UserModel>("UserSesion");
            ResponseAssignment result = new ResponseAssignment();
            DateTime parseStart = new DateTime(1900, 1, 1);
            DateTime parseEnd = new DateTime(2900, 1, 1); ;
            try {
                if (!string.IsNullOrEmpty(model.StartDate)) {
                    DateTime.TryParse(model.StartDate, out parseStart);
                }
                if (!string.IsNullOrEmpty(model.EndDate)) {
                    DateTime.TryParse(model.EndDate, out parseEnd);
                }
                Request request = new Request() {
                    Assignment = new AssignmentModel {
                        UserId = UserSession.Id,
                        TypeConsult = 1,
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
                ViewData["Brand"] = AddListItemDataBrands(dataFilter.Brands);
                if (result.TotalRows == 0) {
                    return PartialView("_WorkOrderList", new List<AssignmentModel>());
                }


            } catch (Exception e) {
                return PartialView("_WorkOrderList", new List<AssignmentModel>());
            }
            return PartialView("_WorkOrderList", result.Assignments.OrderBy(i => i.EndDate).ToList());
        }

        private void KeepFilterData(SearchModel model) {
            HttpContext.Session.SetString("OTFilter", string.IsNullOrEmpty(model.WorkOrderNumber) ? string.Empty : model.WorkOrderNumber.Trim());
            HttpContext.Session.SetString("ClientFilter", string.IsNullOrEmpty(model.Client) ? string.Empty : model.Client.Trim());
            HttpContext.Session.SetString("StartDateFilter", string.IsNullOrEmpty(model.StartDate) ? HttpContext.Session.GetString("StartDateFilter") : model.StartDate.Trim());
            HttpContext.Session.SetString("EndDateFilter", string.IsNullOrEmpty(model.EndDate) ? HttpContext.Session.GetString("EndDateFilter") : model.EndDate.Trim());

        }

        private void OrderPersonalTechnical(ref ResponseAssignment list) {
            try {
                List<AssignmentModel> listNewAssignment = new List<AssignmentModel>();
                if (list.Assignments != null)
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
            } catch (Exception) { }
        }

        private void CalculateActivityDuration(ref AssignmentModel list) {
            List<ActivityModel> listNewAActivity = new List<ActivityModel>();
            foreach (var item in list.Activities) {
                ActivityModel newModel = new ActivityModel();
                List<AssignmentModel> Activities = new List<AssignmentModel>();
                newModel = item;
                newModel.Duration = (item.EndDate - item.StartDate);
                listNewAActivity.Add(newModel);
            }
            list.Activities = listNewAActivity;
        }

        [HttpPost]
        public IActionResult OTDetailReport(string assignmentId) {
            HttpContext.Session.SetString("assignmentId", assignmentId);
            var url = "/ShowDetailByTechnical";
            return Json(new { Url = url }); ;
        }

        public async Task<IActionResult> ShowDetailByTechnical() {
            try {
                var UserSession = HttpContext.Session.Get<UserModel>("UserSesion");
                string userId = UserSession.Id;
                var idAssignment = HttpContext.Session.GetString("assignmentId");
                int rolId = (int)UserSession.UserType;
                AssignmentModel Assig = new AssignmentModel();
                Assig = await _assignmentService.GetReportByIdAsync(rolId.ToString(), idAssignment, userId);
                List<ContactModel> FerreContacts = new List<ContactModel>();
                FerreContacts.AddRange(GetChargeInFerreyrosContact("Supervisor", Assig.FerreyrosContacts));
                FerreContacts.AddRange(GetChargeInFerreyrosContact("Líder", Assig.FerreyrosContacts));
                FerreContacts.AddRange(GetChargeInFerreyrosContact("Técnico", Assig.FerreyrosContacts));
                Assig.FerreyrosContacts = FerreContacts;
                //TempData["Assig"] = Assig;
                HttpContext.Session.Set<AssignmentModel>("DetailTechnical", Assig);
                HttpContext.Session.SetString("TitleForm", "Ordenes de Trabajo");
                HttpContext.Session.SetString("ButtonBack", "BackToOT");
            }catch(Exception){
                return RedirectToAction("WorkOrder", "Reports");
            }
            return RedirectToAction("ReportsDetail", "Reports");
        }

    }

}

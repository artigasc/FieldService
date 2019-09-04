using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FESA.SCM.WebSite.Helpers;
using FESA.SCM.WebSite.Models;
using FESA.SCM.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FESA.SCM.WebSite.Controllers
{
    public class CalendarController : Controller
    {
        private IAssignmentService _assignmentService;
        private static ResponseFilter dataFilter = new ResponseFilter();
        public CalendarController(IAssignmentService assignmentService) {
            _assignmentService = assignmentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Month(int page = 1) {
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
                
            } catch (Exception) {
                return RedirectToAction("Index", "Home");
            }
            return View(result.Assignments);

            //return View();
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

        private void OrderPersonalTechnical(ref ResponseAssignment list) {
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
    }
}
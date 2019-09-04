using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FESA.SCM.WebSite.Models;
using FESA.SCM.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solution.Ferreyros.Models;
using FESA.SCM.WebSite.Helpers;
using FESA.SCM.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FESA.SCM.WebSite.Controllers {
    public class HomeController : Controller {
        private readonly IUserService _userService;

        public HomeController(IUserService userService) {
            _userService = userService;
        }

        public IActionResult Index() {
            try {
                UserModel user = HttpContext.Session.Get<UserModel>("UserSesion");
                if (user != null) {
                    return RedirectToAction("WorkOrder", "OT");
                }
            } catch (Exception) {

            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]UserModel user) {
            string status = "0";
            string userType = "-1";
            UserModel userOnSession = HttpContext.Session.Get<UserModel>("UserSesion");
            try {
                if (userOnSession == null) {
                    UserModel userResponse = await _userService.Login(user.UserName.Trim(), user.Password.Trim());
                    if (userResponse != null) {
                        status = "1";
                        HttpContext.Session.Set<UserModel>("UserSesion", userResponse);
                        HttpContext.Session.SetString("NameUser", userResponse.Name);
                        userType = ConvertUserType.GetStatusUserInt(userResponse.UserType).ToString();
                        return Json(new { content = userResponse, Status = status, userTypeNumber = userType });
                    }
                } else {
                    if (user.UserName.Trim() == userOnSession.UserName && user.Password.Trim() == userOnSession.Password) {
                        status = "1";
                        HttpContext.Session.Set<UserModel>("UserSesion", userOnSession);
                        HttpContext.Session.SetString("NameUser", userOnSession.Name);
                        userType = ConvertUserType.GetStatusUserInt(userOnSession.UserType).ToString();
                    } else {
                        UserModel userResponse = await _userService.Login(user.UserName.Trim(), user.Password.Trim());
                        if (userResponse != null) {
                            status = "1";
                            HttpContext.Session.Set<UserModel>("UserSesion", userResponse);
                            HttpContext.Session.SetString("NameUser", userResponse.Name);
                            userType = ConvertUserType.GetStatusUserInt(userResponse.UserType).ToString();
                            return Json(new { content = userResponse, Status = status, userTypeNumber = userType });
                        }
                    }
                }
            } catch (Exception) {
                HttpContext.Session.Remove("UserSesion");
                return Json(new { content = "null" });
            }
            return Json(new { content = userOnSession, Status = status, userTypeNumber= userType });
        }
        
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult LogOut() {
            string status = "0";
            try {
                status = "1";
                HttpContext.Session.Remove("UserSesion");
            } catch (Exception) {
                return Json(new { content = "Error", Status = status });
            }
            //  return RedirectToAction ("Index");
            return Json(new { content = "Ok", Status = status });
        }
    }
}

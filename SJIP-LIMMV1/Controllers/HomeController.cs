using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Helper;

namespace SJIP_LIMMV1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //default
            ViewBag.UserRoleName = "Guest";
            // Simple verification and redirection to login
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string roleName;
                // further implementations for cutomised dashboard viewmodels
                using (var dbcontext = new ApplicationDbContext())
                {
                    string currentid = HttpContext.User.Identity.GetUserId();
                    string RoleID = dbcontext.Users.Where(user => user.Id == currentid).FirstOrDefault().Roles.FirstOrDefault().RoleId;
                    roleName = dbcontext.Roles.Where(r => r.Id == RoleID).FirstOrDefault().Name;
                }
                ViewBag.UserRoleName = roleName;
                ViewBag.UserGreeting = new CommonFunc().generateGreeting();
                return View();
            }
            else
                return RedirectToAction("Login", "Account");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
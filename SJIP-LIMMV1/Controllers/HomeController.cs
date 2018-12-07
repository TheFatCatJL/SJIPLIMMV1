using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Manager;

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
                //ready the dashboard items
                using (DashboardManager mycomfunc = new DashboardManager())
                {
                    ViewBag.UserRoleName = mycomfunc.generateUserRole(HttpContext);
                    ViewBag.UserGreeting = mycomfunc.generateGreeting();
                }
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
            ViewBag.Title = "Contact Us";
            ViewBag.Message = "Please leave us a message and we will get back to you shortly.";
            ContactFormViewModel model = new ContactFormViewModel();
            //ready the dashboard items
            using (DashboardManager mycomfunc = new DashboardManager())
            {
                model.Name = String.Format("{0} ({1})", mycomfunc.generateUserName(HttpContext), mycomfunc.generateUserRole(HttpContext));
                model.Email = mycomfunc.generateUserEmail(HttpContext);
            }
            return PartialView(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Contact(ContactFormViewModel model)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Title = "Form Submission Error ";
                ViewBag.Message = "Please check the below field(s).";
                return View(model);
            }
            // TBA - To decide on the table details for further implementation
            ViewBag.Title = "Contact Us";
            ViewBag.Message = "Please leave us a message and we will get back to you shortly.";
            return View(model);
        }
    }
}
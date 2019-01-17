using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Manager;
using System.Threading.Tasks;
using SJIP_LIMMV1.Manager.Fluent;

namespace SJIP_LIMMV1.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {

            // Simple verification and redirection to login
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                //ready the dashboard items
                using (DashboardManager mycomfunc = new DashboardManager())
                {
                    ViewData = await mycomfunc.pageViewBagPrep(HttpContext, "Home");
                }
                return View();
            }
            else
            {
                //default
                using (DashboardManager mycomfunc = new DashboardManager())
                {
                    ViewData = await mycomfunc.pageViewBagPrep(HttpContext);
                    return RedirectToAction("Login", "Account");
                }
            }
        }

        public async Task<ActionResult> About()
        {
            using (DashboardManager mycomfunc = new DashboardManager())
            {
                ViewData = await mycomfunc.pageViewBagPrep(HttpContext, "About");
            }
            return View();
        }

        [Authorize]
        public async Task<ActionResult> Contact()
        {
            using (DashboardManager mycomfunc = new DashboardManager())
            {
                ViewData = await mycomfunc.pageViewBagPrep(HttpContext, "Contact");
            }
            ContactFormViewModel model = await new ViewModelManager<ContactFormViewModel>(HttpContext).GetNew();
            return PartialView(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Contact(ContactFormViewModel model)
        {
            if(!ModelState.IsValid)
            {
                using (DashboardManager mycomfunc = new DashboardManager())
                {
                    ViewData = await mycomfunc.pageViewBagPrep(HttpContext, "ContactError");
                }
                return View(model);
            }
            await new ViewModelManager<ContactFormViewModel>(HttpContext).Post(model);
            ContactFormViewModel newmodel = await new ViewModelManager<ContactFormViewModel>(HttpContext).GetNew();
            using (DashboardManager mycomfunc = new DashboardManager())
            {
                ViewData = await mycomfunc.pageViewBagPrep(HttpContext, "ContactAgain");
            }
            return View("Contact",newmodel);
        }
    }
}
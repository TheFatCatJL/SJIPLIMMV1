using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SJIP_LIMMV1.Manager;

namespace SJIP_LIMMV1.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        // GET: Users
        public async Task<ActionResult> Index()
        {
            DashboardManager helper = new DashboardManager();
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;
                ViewBag.displayMenu = "No";

                if(await helper.isAdminUser(user))
                {
                    ViewBag.displayMenu = "Yes";
                }
                helper.Dispose();
                if(Request.IsAjaxRequest())
                {
                    return PartialView("Index");
                }
                return View();
            }
            else
            {
                ViewBag.Name = "Not Logged In";
            }
            helper.Dispose();
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SJIP_LIMMV1.Manager;
using SJIP_LIMMV1.Models;


namespace SJIP_LIMMV1.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        // GET: Roles
        public async Task<ActionResult> Index()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            DashboardManager helper = new DashboardManager();
            if(User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                if(await helper.isAdminUser(user))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            var Roles = dbContext.Roles.ToList();
            return View(Roles);
        }
    }
}
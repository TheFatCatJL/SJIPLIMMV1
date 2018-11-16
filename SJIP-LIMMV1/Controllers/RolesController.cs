using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJIP_LIMMV1.Helper;
using SJIP_LIMMV1.Models;

namespace SJIP_LIMMV1.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        // GET: Roles
        public ActionResult Index()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            CommonFunc helper = new CommonFunc();
            if(User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                if(!helper.isAdminUser(user))
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
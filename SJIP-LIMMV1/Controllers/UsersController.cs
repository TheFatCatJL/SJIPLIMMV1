using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJIP_LIMMV1.Helper;

namespace SJIP_LIMMV1.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            CommonFunc helper = new CommonFunc();
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;
                ViewBag.displayMenu = "No";

                if(helper.isAdminUser(user))
                {
                    ViewBag.displayMenu = "Yes";
                }
                helper.Dispose();
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
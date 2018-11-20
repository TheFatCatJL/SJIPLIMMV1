using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJIP_LIMMV1.Models;



namespace SJIP_LIMMV1.Controllers
{
    [Authorize]
    public class BoxInfoController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            
            if (User.Identity.IsAuthenticated)
            {
                
                return View(new BoxInfoViewModel());
            }
            else
            {
                ViewBag.Name = "Not Logged In";
            }
           
            return View();
        }
    }
}
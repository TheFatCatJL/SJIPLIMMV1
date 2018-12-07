using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJIP_LIMMV1.Manager;

namespace SJIP_LIMMV1.Controllers
{
    public class RedirectionController : Controller
    {
        // GET: Redirection
        [Route("HandleErrorRedirection")]
        public ActionResult Index()
        {
            ViewData = new DashboardManager().errorViewBagPrep(HttpContext.Request.RequestContext, "DeletedEditEntry");
            return View("Error");
        }
    }
}
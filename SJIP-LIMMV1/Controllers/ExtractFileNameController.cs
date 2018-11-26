using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SJIP_LIMMV1.Controllers
{
    public class ExtractFileNameController : Controller
    {
        // GET: NameExtraction
        [HttpGet]
        public ActionResult NameExtraction()
        {
            return View(new List<HttpPostedFileWrapper>());
        }

        [HttpPost]
        public ActionResult NameExtraction(List<HttpPostedFileWrapper> postedFiles)
        {
            if (postedFiles != null)
            {
                ArrayList list = new ArrayList();
                
                Debug.WriteLine(postedFiles.Count());
                foreach (var file in postedFiles)
                {
                    
                    list.Add(Path.GetFileNameWithoutExtension(file.FileName));
                }
                
                ViewBag.FileNames = list;
                ViewBag.Message = postedFiles.Count() + " Files uploaded successfully.";
            }

            return View();
        }
    }
}
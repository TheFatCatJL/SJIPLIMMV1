using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SJIP_LIMMV1.Models;
using PagedList;

namespace SJIP_LIMMV1.Controllers
{
    public class SearchController : Controller
    {
        LiftInstallationDataDBEntities1 db = new LiftInstallationDataDBEntities1();


        // GET: Search
        //[Route("/search/createView/{searchViewModel}")]
        public ActionResult createView()
        {                       
            SearchViewModel searchViewModel = new SearchViewModel();
            
            searchViewModel.SensorBoxInfoResults = db.SensorBoxInfoes.ToList();
            PagedList<SensorBoxInfo> pagedModel = new PagedList<SensorBoxInfo>(searchViewModel.SensorBoxInfoResults, 1, 2);
            
            searchViewModel.PagedSensorBoxInfo = pagedModel;   
            
            //RedirectToAction("submitSearch");
            //PagedList <SensorBoxInfo> = new PagedList<SensorBoxInfo>(pageNumber, pageSize);
            return View(searchViewModel);
        }
        [HttpPost]
        //[Route("/search/submitSearch")]
        public JsonResult submitSearch(SearchViewModel searchViewModel, int? page)
        {

            var sensorBoxInfo = db.SensorBoxInfoes.ToList();

            if (searchViewModel.Block != null)
            {
                sensorBoxInfo = sensorBoxInfo.Where(x => (x.BlockNo.ToString()).StartsWith((searchViewModel.Block.ToString().Trim()))).ToList();
            }
            if (searchViewModel.TownCouncil != null)
            {
                sensorBoxInfo = sensorBoxInfo.Where(x => x.TownCouncil.ToLower().Contains(searchViewModel.TownCouncil.Trim().ToLower())).ToList();
            }
            if (searchViewModel.SIMCard != null)
            {
                sensorBoxInfo = sensorBoxInfo.Where(x => x.SIMCard.ToLower().StartsWith(searchViewModel.SIMCard.Trim().ToLower())).ToList();
            }
            if (searchViewModel.LMPD != null)
            {
                sensorBoxInfo = sensorBoxInfo.Where(x => x.LMPD.ToLower().StartsWith(searchViewModel.LMPD.Trim().ToLower())).ToList();
            }
         
            searchViewModel.SensorBoxInfoResults = sensorBoxInfo;
            int pageSize = 2;

            int pageNumber = (page ?? 1);
            searchViewModel.PagedSensorBoxInfo = new PagedList<SensorBoxInfo>(searchViewModel.SensorBoxInfoResults, pageNumber, pageSize);

            //return View("_SearchResult", searchViewModel.PagedSensorBoxInfo);
            return Json(searchViewModel.SensorBoxInfoResults, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult pagedResult( int? page)
        {

            SearchViewModel searchViewModel = new SearchViewModel();

            var sensorBoxInfo = db.SensorBoxInfoes.ToList();

            searchViewModel.SensorBoxInfoResults = sensorBoxInfo;
            int pageSize = 2;

            int pageNumber = (page ?? 1);
            searchViewModel.PagedSensorBoxInfo = new PagedList<SensorBoxInfo>(searchViewModel.SensorBoxInfoResults, pageNumber, pageSize);

            return View("_SearchResult", searchViewModel);
           
            

        }



    }
}
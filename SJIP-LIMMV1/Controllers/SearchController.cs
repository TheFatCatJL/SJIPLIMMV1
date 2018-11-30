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
        static List<SensorBoxInfo> currentSensorBoxInfoList;
        //static int currentPageSize;
        //static int currentPageNumber;

        // GET: Search
        public ActionResult createView()
        {                       
            SearchViewModel searchViewModel = new SearchViewModel();

            currentSensorBoxInfoList = db.SensorBoxInfoes.ToList();

            //set initial page number and size
            
            PagedList<SensorBoxInfo> pagedModel = new PagedList<SensorBoxInfo>(currentSensorBoxInfoList, searchViewModel.defaultPageNumber, searchViewModel.defaultPageSize);

            searchViewModel.PagedSensorBoxInfo = pagedModel;    
            return View(searchViewModel);
        }

        [HttpPost]        
        public ActionResult submitSearch(SearchViewModel searchViewModel)
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
            
            currentSensorBoxInfoList = sensorBoxInfo;

            searchViewModel.PagedSensorBoxInfo = new PagedList<SensorBoxInfo>(currentSensorBoxInfoList, searchViewModel.defaultPageNumber, searchViewModel.defaultPageSize);

            return PartialView("_SearchResult", searchViewModel);
        }

        [HttpGet]
        public ActionResult pagedResult(int size, int? page)
        {

            SearchViewModel searchViewModel = new SearchViewModel();

            int pageNumber = (page ?? 1);
          
            searchViewModel.PagedSensorBoxInfo = new PagedList<SensorBoxInfo>(currentSensorBoxInfoList, pageNumber, size);

            return PartialView("_SearchResult", searchViewModel);           
        }



    }
}
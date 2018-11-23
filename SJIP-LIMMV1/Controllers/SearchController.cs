using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SJIP_LIMMV1.Models;

namespace SJIP_LIMMV1.Controllers
{
    public class SearchController : Controller
    {



        // GET: Search
        //[Route("/search/createView/{searchViewModel}")]
        public ActionResult createView()
        {

            LiftInstallationDataDBEntities1 db = new LiftInstallationDataDBEntities1();
            SearchViewModel searchViewModel = new SearchViewModel();


            searchViewModel.SensorBoxInfoResults = db.SensorBoxInfoes.ToList();

            return View(searchViewModel);
        }
        [HttpPost]
        //[Route("/search/submitSearch")]
        public async Task<ActionResult> submitSearch(SearchViewModel searchViewModel)
        {

            LiftInstallationDataDBEntities1 db = new LiftInstallationDataDBEntities1();
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


            //return View("createView", searchViewModel);
            return Json(searchViewModel.SensorBoxInfoResults, JsonRequestBehavior.AllowGet);

        }

    }
}
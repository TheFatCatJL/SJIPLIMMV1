using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SJIP_LIMMV1.Manager;
using SJIP_LIMMV1.Models;

namespace SJIP_LIMMV1.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public async Task<ActionResult> MainSearch()
        {
            SearchViewModel spvm = (SearchViewModel) await new SearchManager("Search").GetModel();
            spvm.radiochoice = "1";
            return View(spvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MainSearch(SearchViewModel spvm)
        {
            if(!ModelState.IsValid)
            {
                return PartialView("_SearchPartialGrid");
            }
            spvm = (SearchViewModel)await new SearchManager("Search").PostModel(spvm);
            BoxInfoRecord record = spvm.records;
            List<BoxInfo> boxlist = record.Cast<BoxInfo>().ToList();
            return PartialView("_SearchPartialGrid", boxlist);
        }

        public ActionResult MainSearchGrid(string searchstring)
        {
            SearchViewModel spvm = new SearchViewModel();
            spvm.searchstring = searchstring;
            spvm = (SearchViewModel)Task.Run(() => new SearchManager("Search").PostModel(spvm)).Result;
            BoxInfoRecord record = spvm.records;
            List<BoxInfo> boxlist = record.Cast<BoxInfo>().ToList();
            return PartialView("_SearchPartialGrid", boxlist);
        }

        // Building SPVM for content div ajax operations
        [HttpPost]
        public ActionResult GetSearchStringAjax(string searchstring)
        {
            SearchViewModel spvm = new SearchViewModel();
            spvm.searchstring = searchstring;
            return Json(spvm);
        }

        // Ajax for Pbox content div
        [HttpPost]
        public async Task<ActionResult> PboxSearchAjax(SearchViewModel model)
        {
            SearchViewModel spvm = (SearchViewModel)await new SearchManager("Search").GetModel();
            spvm.searchstring = model.searchstring;
            PreBoxInfoViewModel pbvm = (PreBoxInfoViewModel)await new SearchManager("Search").PostModel(new PreBoxInfoViewModel(), spvm, HttpContext);
            return PartialView("_PreBoxSearchPartial", pbvm);
        }

        // Ajax for Cbox content div
        [HttpPost]
        public async Task<ActionResult> CboxSearchAjax(SearchViewModel model)
        {
            SearchViewModel spvm = (SearchViewModel)await new SearchManager("Search").GetModel();
            spvm.searchstring = model.searchstring;
            ComBoxInfoViewModel cbvm = (ComBoxInfoViewModel)await new SearchManager("Search").PostModel(new ComBoxInfoViewModel(), spvm, HttpContext);
            return PartialView("_ComBoxSearchPartial", cbvm);
        }

        // Ajax for ComRec content div
        [HttpPost]
        public async Task<ActionResult> ComRecSearchAjax(SearchViewModel model)
        {
            SearchViewModel spvm = (SearchViewModel)await new SearchManager("Search").GetModel();
            spvm.searchstring = model.searchstring;
            CommissionRecordVM crvm = (CommissionRecordVM)await new SearchManager("Search").PostModel(new CommissionRecordVM(), spvm, HttpContext);
            return PartialView("_ComRecSearchPartial", crvm);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SJIP_LIMMV1.Manager;
using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Repository;
using SJIP_LIMMV1.CommonFunctions;

namespace SJIP_LIMMV1.Controllers
{
    [Authorize]
    public class BoxInfoController : Controller
    {             
        // Declare logger - Note standard declaration
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region BoxInfoSection - the Form
        // ~/BoxInfo/BoxInfoSectionPartial - Ajax Method
        public async Task<ActionResult> BoxInfoSectionPartial()
        {
            if (Request.IsAjaxRequest())
            {
                logger.Info("BoxInfo PartialView loaded");
                BoxInfoViewModel vm = (BoxInfoViewModel) await new ViewModelManager("BoxInfo", false).GetModel(null);
                return PartialView("_BoxInfoSectionPartial", vm);
            }
            return RedirectToActionPermanent("BoxInfoSection");
        }

        // ~/BoxInfo/BoxInfoModule - Full View
        public async Task<ActionResult> BoxInfoSection()
        {
            logger.Info("BoxInfo FullView loaded");
            BoxInfoViewModel vm = (BoxInfoViewModel) await new ViewModelManager("BoxInfo", false).GetModel(null);
            return View("BoxInfoSection", vm);
        }

        #endregion

        #region BoxHistorySection - All about the grid

        // ~/BoxInfo/BoxHistorySectionPartial
        public async Task<ActionResult> BoxHistorySectionPartial()
        {
            if (Request.IsAjaxRequest())
            {
                IEnumerable<BoxInfoViewModel> model = (IEnumerable <BoxInfoViewModel> ) await new ViewModelManager("BoxInfos", true).GetModels();
                logger.Info("BoxHistory PartialView loaded");
                return PartialView("_BoxHistorySectionPartial", model);                
            }
            return BoxHistorySection();
        }

        // ~/BoxInfo/BoxHistorySection
        public ActionResult BoxHistorySection()
        {
            IEnumerable<BoxInfoViewModel> model = (IEnumerable<BoxInfoViewModel>) new ViewModelManager("BoxInfos", true).GetModels();
            logger.Info("BoxHistory FullView loaded");
            return View("BoxHistorySection", model);
        }

        #endregion

        #region BoxInfo Editing on the modal
        // ~/BoxInfo/BoxEditSectionSetup
        public async Task<ActionResult> BoxEditSectionSetup()
        {
            // Passing null for ViewModelManager to manage a new creation
            BoxInfoViewModel vm = (BoxInfoViewModel) await new ViewModelManager("BoxInfo", false).GetModel(null);
            logger.Info("BoxEdit PartialView loaded");
            return PartialView("_BoxEditSectionPartial", vm);        
        }

        // ~/BoxInfo/BoxEditSectionGetBox
        public async Task<ActionResult> BoxEditSectionGetBox(int? id)
        {

            BoxInfoViewModel vm = (BoxInfoViewModel)await new ViewModelManager("BoxInfo", false).GetModel(id);
            if (vm != null)
            {
                logger.Info("BoxEdit of id : " + id + " loaded");
                return PartialView("_BoxEditSectionPartial", vm);
            }

            // When VM is null, there should be an error / no data which details will be logged at manager/repo level
            // Consider making an ErrorManager to handle error traffic and redirects properly
            logger.Error("Unknown error causing BoxInfo id to be null at BoxHistory dashboard");
            return RedirectToActionPermanent("BoxEditSectionSetup");
        }

        #endregion

        #region Box Posting from modal (PUT) and from form (POST)
        
        // ~/BoxInfo/SubmitBox
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitBox(BoxInfoViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                await new ViewModelManager("BoxInfo", false).PostModel(viewmodel);
                BoxInfoViewModel newViewModel = (BoxInfoViewModel) await new ViewModelManager("BoxInfo", false).GetModel(null);
                return View("BoxInfoSection", newViewModel);
            }
            return View("BoxInfoSection", viewmodel);
        }

        // Route will be ~/BoxInfo/SubmitBoxAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitBoxAjax(BoxInfoViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                await new ViewModelManager("BoxInfo", false).PostModel(viewmodel);
                BoxInfoViewModel newViewModel = (BoxInfoViewModel) await new ViewModelManager("BoxInfo", false).GetModel(null);
                return PartialView("BoxInfoSectionPartial", newViewModel);
            }
            return PartialView("BoxInfoSectionPartial", viewmodel);
        }

        #endregion
    }
}
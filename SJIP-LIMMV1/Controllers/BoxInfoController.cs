using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using SJIP_LIMMV1.Common;
using SJIP_LIMMV1.Manager;
using SJIP_LIMMV1.Models;

namespace SJIP_LIMMV1.Controllers
{
    [Authorize]
    public class BoxInfoController : Controller
    {             
        // Declare logger - Note standard declaration
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region PREBOX

        #region PreCommission - Pre-commission Form - AJAX and FULL
        // ~/BoxInfo/PreBoxAddPartial - Ajax Method
        public async Task<ActionResult> PreBoxAddPartial()
        {
            if (Request.IsAjaxRequest())
            {
                logger.Info("PreBoxAdd PartialView loaded");
                PreBoxInfoViewModel vm = await new ViewModelManager<PreBoxInfoViewModel>(HttpContext).GetNew();
                return PartialView("_PreBoxAddPartial", vm);
            }
            return RedirectToActionPermanent("PreBoxAdd");
        }

        // ~/BoxInfo/PreBoxAdd - Full View
        public async Task<ActionResult> PreBoxAdd()
        {
            logger.Info("PreBoxAdd FullView loaded");
            PreBoxInfoViewModel vm = await new ViewModelManager<PreBoxInfoViewModel>(HttpContext).GetNew();
            return View("PreBoxAdd", vm);
        }

        #endregion

        #region PreBoxHistory - Summary of Pre-commission data (Mainly Read option methods)

        // ~/BoxInfo/PreBoxHistoryPartial
        public async Task<ActionResult> PreBoxHistoryPartial()
        {
            if (Request.IsAjaxRequest() || HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                IEnumerable<PreBoxInfoViewModel> models = await new ViewModelManager<PreBoxInfoViewModel>(HttpContext).GetAll();
                logger.Info("PreBoxHistoryGRID loaded");
                return PartialView("_PreBoxHistoryPartialGRID", models);                
            }
            logger.Info("NonAjax request accessing PreBoxHistoryGrid- redirected to Full view");
            return PreBoxHistory();
        }

        // ~/BoxInfo/PreBoxHistory
        public ActionResult PreBoxHistory()
        {
            IEnumerable<PreBoxInfoViewModel> models = new ViewModelManager<PreBoxInfoViewModel>(HttpContext).GetAll().Result;
            logger.Info("PreBoxHistory FullView loaded");
            return View("PreBoxHistory", models);
        }

        #endregion

        #region PreBoxInfo - Modal popup setup
        // ~/BoxInfo/PreBoxModalSetup
        public async Task<ActionResult> PreBoxModalSetup(bool isNew = false) //note default param
        {
            PreBoxInfoViewModel vm = await new ViewModelManager<PreBoxInfoViewModel>(HttpContext).GetNew();
            if (isNew) // Different partial view return to Modal for Add and Edit
            { 
                logger.Info("PreBoxModalSetup (NEW) PartialView loaded");
                return PartialView("_PreBoxAddPartial", vm);
            }
            logger.Info("PreBoxModalSetup (EDIT) PartialView loaded");
            return PartialView("_PreBoxEditPartial", vm);        
        }

        // For overcoming child action restrictions
        public ActionResult PreBoxModalSetupNonAsync (bool isNew = false)
        {
            PreBoxInfoViewModel vm = new PreBoxInfoViewModel();
            vm.status = "Pre-Commission";
            return PartialView("_PreBoxAddPartial", vm);
        }

        // ~/BoxInfo/PreBoxEditGet
        public async Task<ActionResult> PreBoxEdit(int? id)
        {
            int nullableID = id == null ? -1 : (int)id;
            PreBoxInfoViewModel vm = await new ViewModelManager<PreBoxInfoViewModel>(HttpContext).GetByID(nullableID);
            if (vm != null)
            {
                logger.Info("BoxEdit of id : " + id + " loaded");
                return PartialView("_PreBoxEditPartial", vm);
            }
            // When VM is null, there should be an error / no data which details will be logged at manager/repo level
            // Consider making an ErrorManager to handle error traffic and redirects properly
            logger.Error("Unknown error causing BoxInfo id to be null at BoxHistory dashboard");
            return RedirectToActionPermanent("PreBoxModalSetup");
        }

        // ~/BoxInfo/BoxEditSectionGetBox
        public async Task<ActionResult> PreBoxDelete(int id)
        {
            try
            {
                await new ViewModelManager<PreBoxInfoViewModel>(HttpContext).Delete(id);
                logger.Info("PreBox of id : " + id + " deleted");
                // Passing null for ViewModelManager to manage a new creation
                List<PreBoxInfoViewModel> vms = (List<PreBoxInfoViewModel>)await new ViewModelManager<PreBoxInfoViewModel>(HttpContext).GetAll();
                return PartialView("_PreBoxHistoryPartialGRID", vms);
            }
            catch(Exception e)
            {
                // When VM is null, there should be an error / no data which details will be logged at manager/repo level
                // Consider making an ErrorManager to handle error traffic and redirects properly
                logger.Error("Unknown error causing PreBox id to be null. Error Trace: "+e);
                return RedirectToActionPermanent("PreBoxHistory");
            }
        }

        #endregion

        #region PreBoxInfo - Posting from modal (PUT/POST) and from form (POST)

        // ~/BoxInfo/PreBoxSubmit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PreBoxSubmit(PreBoxInfoViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                await new ViewModelManager<PreBoxInfoViewModel>(HttpContext).Post(viewmodel);
                PreBoxInfoViewModel newViewModel = await new ViewModelManager<PreBoxInfoViewModel>(HttpContext).GetNew();
                return View("PreBoxAdd", newViewModel);
            }
            return View("PreBoxAdd", viewmodel);
        }

        // ~/BoxInfo/PreBoxSubmitAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PreBoxSubmitAjax(PreBoxInfoViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                await new ViewModelManager<PreBoxInfoViewModel>(HttpContext).Post(viewmodel);
                IEnumerable<PreBoxInfoViewModel> newViewModel = await new ViewModelManager<PreBoxInfoViewModel>(HttpContext).GetAll();
                return PartialView("_PreBoxHistoryPartialGRID", newViewModel);
            }
            IEnumerable<PreBoxInfoViewModel> oldViewModel = await new ViewModelManager<PreBoxInfoViewModel>(HttpContext).GetAll();
            return PartialView("_PreBoxHistoryPartialGRID", oldViewModel);
        }

        #endregion

        #endregion

        #region COMBOX

        #region ComBox - Commissioned Box Reporting Form Setup - Ajax and Full

        // Combox GRID PartialView 
        public ActionResult ComBoxPartialGRID(int specialparams)
        {
            IEnumerable<ComBoxInfoViewModel> models = Task.Run(() => new ViewModelManager<ComBoxInfoViewModel>(HttpContext).GetBySpecial(specialparams)).Result;
            logger.Info("ComBoxGRID loaded");
            return PartialView("_ComBoxPartialGRID", models);
        }

        // ~/BoxInfo/ComBoxAddPartial - Ajax Method
        public async Task<ActionResult> ComBoxAddPartial(int ID = 0)
        {
            if (Request.IsAjaxRequest())
            {
                logger.Info("COmBoxAdd PartialView loaded");
                ComBoxInfoViewModel vm = await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).GetNew();
                vm.comrecId = ID;
                return PartialView("_ComBoxAddPartial", vm);
            }
            return RedirectToActionPermanent("ComBoxAdd");
        }

        // ~/BoxInfo/PreBoxAdd - Full View
        public async Task<ActionResult> ComBoxAdd()
        {
            logger.Info("PreBoxAdd FullView loaded");
            ComBoxInfoViewModel vm = await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).GetNew();
            return View("ComBoxAdd", vm);
        }

        #endregion

        #region ComBoxInfo - Modal popup setup
        // ~/BoxInfo/ComBoxModalSetup
        public async Task<ActionResult> ComBoxModalSetup(bool isNew = false) //note default param
        {
            ComBoxInfoViewModel vm = await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).GetNew();
            if (isNew) // Different partial view return to Modal for Add and Edit
            {
                logger.Info("ComBoxModalSetup (NEW) PartialView loaded");
                return PartialView("_ComBoxAddPartial", vm);
            }
            logger.Info("ComBoxModalSetup (EDIT) PartialView loaded");
            return PartialView("_ComBoxEditPartial", vm);
        }

        // Temp for overcoming child action restrictions
        public ActionResult ComBoxModalSetupNonAsync(int ID, bool isNew = false)
        {
            ComBoxInfoViewModel vm = Task.Run(async () => await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).GetNew()).Result;
            vm.comrecId = ID;
            if(ID !=0)
                vm.comrec = Task.Run(async () => await new ViewModelManager<CommissionRecordVM>(HttpContext).GetByID(ID)).Result;
            //sets up dropdownlist
            SelectListBuilder.Setup(HttpContext);

            if (isNew) // Different partial view return to Modal for Add and Edit
            {
                logger.Info("ComBoxModalSetup (NEW) PartialView loaded");
                return PartialView("_ComBoxAddPartial", vm);
            }
            logger.Info("ComBoxModalSetup (EDIT) PartialView loaded");
            return PartialView("_ComBoxAddPartial", vm);
        }

        // ~/BoxInfo/ComBoxEdit
        public async Task<ActionResult> ComBoxEdit(int? id)
        {
            int nullableID = id == null ? -1 : (int)id;
            ComBoxInfoViewModel vm = await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).GetByID(nullableID);
            logger.Info("BoxEdit of id : " + id + " loaded");
            return PartialView("_ComBoxEditPartial", vm);
        }

        // ~/BoxInfo/ComBoxDelete
        public async Task<ActionResult> ComBoxDelete(int id)
        {
            try
            {
                ComBoxInfoViewModel cbvm = await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).GetByID(id);
                SelectListBuilder.Push(cbvm.lmpdnum);
                await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).Delete(id);
                logger.Info("PreBox of id : " + id + " deleted");
                List<ComBoxInfoViewModel> vms = (List<ComBoxInfoViewModel>)await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).GetAll();
                return PartialView("_ComBoxPartialGRID", vms);
            }
            catch (Exception e)
            {
                // When VM is null, there should be an error / no data which details will be logged at manager/repo level
                // Consider making an ErrorManager to handle error traffic and redirects properly
                logger.Error("Unknown error causing ComBox id to be null. Error Trace: " + e);
                List<ComBoxInfoViewModel> vms = (List<ComBoxInfoViewModel>)await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).GetAll();
                return PartialView("_ComBoxPartialGRID", vms);
            }
        }

        #endregion

        #region ComBoxInfo - Posting from modal (PUT/POST) and from form (POST)

        // ~/BoxInfo/ComBoxSubmit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ComBoxSubmit(ComBoxInfoViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).Post(viewmodel);
                SelectListBuilder.Pop(viewmodel.lmpdnum);
                ComBoxInfoViewModel newViewModel = await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).GetNew();
                return View("_ComBoxAddPartial", newViewModel);
            }
            return View("_ComBoxAddPartial", viewmodel);
        }

        // Route will be ~/BoxInfo/ComBoxSubmitAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ComBoxSubmitAjax(ComBoxInfoViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                if (viewmodel.comrecId != 0)
                {
                    viewmodel.comrec = await new ViewModelManager<CommissionRecordVM>(HttpContext).GetByID(viewmodel.comrecId);
                }
                await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).Post(viewmodel);
                SelectListBuilder.Pop(viewmodel.lmpdnum);
                CommissionRecordVM comrec = await new ViewModelManager<CommissionRecordVM>(HttpContext).GetOneBySpecial(viewmodel.lmpdnum);                
                return PartialView("_ComBoxPartialGRID", comrec.vms);
            }
            IEnumerable<ComBoxInfoViewModel> oldViewModel = await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).GetAll();
            return PartialView("_ComBoxPartialGRID", oldViewModel);
        }

        #endregion

        #endregion
        
        #region COMREC

        // ~/BoxInfo/ComRecForm - Essentially CREATE COM REC Form
        public async Task<ActionResult> ComRecForm()
        {
            CommissionRecordVM vm = await new ViewModelManager<CommissionRecordVM>(HttpContext).GetNew();
            return View(vm);
        }

        // ~/BoxInfo/ComRecFormGet/?
        public async Task<ActionResult> ComRecFormGet(int comrecid = 0)
        {
            if(comrecid == 0)
            {
                return RedirectToActionPermanent("ComRecForm");
            }
            CommissionRecordVM vm = await new ViewModelManager<CommissionRecordVM>(HttpContext).GetByID(comrecid);
            if(vm.status == "Report Pending")
            {
                return View("ComRecForm", vm);
            }
            return View("ComRecEdit", vm);
        }

        public async Task<ActionResult> ComRecFormPartial()
        {
            if (Request.IsAjaxRequest())
            {
                CommissionRecordVM vm = await new ViewModelManager<CommissionRecordVM>(HttpContext).GetNew();
                return PartialView("_ComBoxPartialGRID", vm);
            }
            return RedirectToActionPermanent("ComRecForm");
        }

        public async Task<ActionResult> ComRecHistory()
        {
            List<CommissionRecordVM> vms = (List<CommissionRecordVM>)await new ViewModelManager<CommissionRecordVM>(HttpContext).GetAll();
            return View("ComRecHistory", vms);
        }

        // ~/BoxInfo/ComRecEdit
        public async Task<ActionResult> ComRecEdit(CommissionRecordVM vmodel)
        {
            IEnumerable<ComBoxInfoViewModel> models = await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).GetBySpecial(vmodel.Id);
            vmodel.vms = models?.ToList();
            if (ModelState.IsValid & vmodel.vms != null)
            {
                await new ViewModelManager<CommissionRecordVM>(HttpContext).Post(vmodel);
                logger.Info("Commission Record loaded");
                return View("ComRecEdit", vmodel);
            }
            return RedirectToActionPermanent("ComRecHistory");
        }



        // ~/BoxInfo/ComBoxDelete
        [HttpPost]
        public async Task<ActionResult> ComRecDelete(int id)
        {
            try
            {
                await new ViewModelManager<CommissionRecordVM>(HttpContext).Delete(id);
                logger.Info("Commission Record of id : " + id + " deleted");
                List<CommissionRecordVM> vms = (List<CommissionRecordVM>)await new ViewModelManager<CommissionRecordVM>(HttpContext).GetAll();
                return PartialView("ComRecHistory", vms);
            }
            catch (Exception e)
            {
                logger.Error("Unknown error causing Commission Record ID to be null. Error Trace: " + e);
                List<CommissionRecordVM> vms = (List<CommissionRecordVM>)await new ViewModelManager<CommissionRecordVM>(HttpContext).GetAll();
                return PartialView("ComRecHistory", vms);
            }
        }

        // ~/BoxInfo/ComBoxSubmit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ComRecSubmit(CommissionRecordVM vmodel)
        {
            IEnumerable<ComBoxInfoViewModel> models = await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).GetBySpecial(vmodel.Id);
            vmodel.vms = models?.ToList();
            if (ModelState.IsValid & vmodel.vms !=null)
            {
                await new ViewModelManager<CommissionRecordVM>(HttpContext).Post(vmodel);
                CommissionRecordVM newViewModel = await new ViewModelManager<CommissionRecordVM>(HttpContext).GetByID(vmodel.Id);
                return View("ComRecEdit", newViewModel);
            }
            return View("ComRecForm", vmodel);
        }

        // Route will be ~/BoxInfo/ComBoxSubmitAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ComRecSubmitAjax(CommissionRecordVM viewmodel)
        {
            IEnumerable<ComBoxInfoViewModel> models = await new ViewModelManager<ComBoxInfoViewModel>(HttpContext).GetBySpecial(viewmodel.Id);
            viewmodel.vms = models?.ToList();
            if (ModelState.IsValid & viewmodel.vms != null)
            {
                await new ViewModelManager<CommissionRecordVM>(HttpContext).Post(viewmodel);
                IEnumerable<CommissionRecordVM> newViewModel = await new ViewModelManager<CommissionRecordVM>(HttpContext).GetAll();
                return View("ComRecHistory", newViewModel);
            }
            return View("ComRecForm", viewmodel);
        }

        #endregion
    }
}
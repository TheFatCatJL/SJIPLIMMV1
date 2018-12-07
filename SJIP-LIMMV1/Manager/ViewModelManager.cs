using System;
using System.Collections.Generic;
using SJIP_LIMMV1.Repository;
using SJIP_LIMMV1.Models.Interfaces;
using SJIP_LIMMV1.Models;
using Newtonsoft.Json;
using System.Web.Http.Results;
using System.Threading.Tasks;
using System.Web;
using System.Runtime.InteropServices;

namespace SJIP_LIMMV1.Manager
{
    public class ViewModelManager : IDisposable
    {
        private IViewModel baseModel;
        private IEnumerable<IViewModel> baseModels;
        private string errorMsg = "";
        private HttpContextBase context;

        // Declare logger - Note standard declaration
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Makes ViewModel
        private static IDictionary<string, Func<IViewModel>> MakeBaseModel = new Dictionary<string, Func<IViewModel>>
        {
            {
                "BoxInfo", ()=> new BoxInfoViewModel()
            }
        };

        // Makes List<ViewModel>
        private static IDictionary<string, Func<IEnumerable<IViewModel>>> MakeBaseModels = new Dictionary<string, Func<IEnumerable<IViewModel>>>
        {
            {
                "BoxInfos", ()=> new List<BoxInfoViewModel>()
            }
        };

        // Constructor takes in the VM order and if the order requires a list
        public ViewModelManager(string vmtype, bool isList, [Optional] HttpContextBase context)
        {
            if(isList)
            {
                var baseModelsOrder = MakeBaseModels[vmtype];
                this.baseModels = baseModelsOrder.Invoke();                
            }
            else
            {
                var baseModelOrder = MakeBaseModel[vmtype];
                this.baseModel = baseModelOrder.Invoke();
            }
            this.context = context;
        }


        #region Get Methods

        // Type is inferred from constructor
        public async Task<IViewModel> GetModel(int? id)
        {
            if (baseModel.GetType() == typeof(BoxInfoViewModel))                
                return await MakeBoxInfoVM(id);
            if (baseModel.GetType() == typeof(ContactFormViewModel))
                return MakeContactFormVM();

            // if we gone this far, something is wrong
            errorMsg = "Error";
            return null;
        }

        // Type is inferred from constructor
        public async Task<IEnumerable<IViewModel>> GetModels()
        {
            if (baseModels.GetType() == typeof(List<BoxInfoViewModel>))
                return await MakeBoxInfoListVM();
            if (baseModel.GetType() == typeof(ContactFormViewModel))
                return null;

            // if we gone this far, something is wrong
            errorMsg = "Error";
            return null;
        }

        #endregion

        #region Post Methods

        internal async Task PostModel(IViewModel vm)
        {
            if (baseModel.GetType() == typeof(BoxInfoViewModel))
                await PostBoxInfoVM(vm);
            if (baseModel.GetType() == typeof(ContactFormViewModel))
                await PostContactFormVM();

            // if we gone this far, something is wrong
            errorMsg = "Error";
        }

        #endregion

        #region Delete Methods

        public async Task DeleteModel(int id)
        {
            if (baseModel.GetType() == typeof(BoxInfoViewModel))
                await DeleteBoxInfoVM(id);
            if (baseModel.GetType() == typeof(ContactFormViewModel))
                await DeleteContactFormVM();

            // if we gone this far, something is wrong
            errorMsg = "Error";
        }

        #endregion

        #region BoxInfoVM Operations
        private async Task<BoxInfoViewModel> MakeBoxInfoVM(int? orderid)
        {
            try
            {
                if (orderid is null)
                {
                    return new BoxOperationsManager("BoxInfoInitial").getPackage();
                }
                else
                {
                    int id = (int)orderid;
                    var result = await GetBoxInfoRepo().GetBoxInfoViewModel(id);
                    baseModel = result;
                    return (BoxInfoViewModel)baseModel;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private async Task<IEnumerable<BoxInfoViewModel>> MakeBoxInfoListVM()
        {
            try
            {
                var result = await GetBoxInfoRepo().GetBoxInfoViewModels();
                baseModels = result;
                return (List<BoxInfoViewModel>) baseModels;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private async Task PostBoxInfoVM(IViewModel vm)
        {
            BoxInfoViewModel boxInfoViewModel = (BoxInfoViewModel) vm;
            boxInfoViewModel.lmpdnum = string.IsNullOrEmpty(boxInfoViewModel.lmpdnum) ? null : boxInfoViewModel.lmpdnum.Trim().ToUpper();
            var findLMPD = string.IsNullOrEmpty(boxInfoViewModel.lmpdnum) ? null : await GetBoxInfoRepo().GetBoxInfoViewModel( boxInfoViewModel.lmpdnum);
            if (findLMPD != null)
            {
                await GetBoxInfoRepo().PutBoxInfoViewModel(boxInfoViewModel);
            }
            else
            {
                IDictionary<string, object> orderdetails = new Dictionary<string, object>();
                orderdetails.Add("checkername", new DashboardManager().generateUserName(context));
                boxInfoViewModel = new BoxOperationsManager("BoxSubmmission", orderdetails, boxInfoViewModel).getPackage();
                await GetBoxInfoRepo().PostBoxInfoViewModel(boxInfoViewModel);
            }
        }

        private async Task DeleteBoxInfoVM(int id)
        {
            if (GetBoxInfoRepo().BoxInfoViewModelExists(id))
            {
                await GetBoxInfoRepo().DeleteBoxInfoViewModel(id);
            }
            else
            {
                logger.Info("Attempt to delete BoxInfoVM id(" + id + ") failed as there is no such ID in database");
            }
        }

        private BoxInfoRepo GetBoxInfoRepo()
        {
            return new BoxInfoRepo();
        }

        #endregion

        #region ContactFormVM Operations
        internal ContactFormViewModel MakeContactFormVM()
        {
            return null;
        }

        internal async Task PostContactFormVM()
        {
            
        }

        internal async Task DeleteContactFormVM()
        {
            
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
               
    }
}
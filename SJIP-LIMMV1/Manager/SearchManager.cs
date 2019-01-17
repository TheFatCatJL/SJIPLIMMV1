using System;
using System.Collections.Generic;
using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Models.Interfaces;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SJIP_LIMMV1.Repository;

namespace SJIP_LIMMV1.Manager
{
    public class SearchManager
    {
        private BoxInfoRepo repo = new BoxInfoRepo();
        
        // Declare logger - Note standard declaration
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Makes ViewModel
        private static IDictionary<string, Func<IViewModel>> MakeBaseModel = new Dictionary<string, Func<IViewModel>>
        {
            { "Search", ()=> new SearchViewModel() }
        };

        // Inherit parent constructor
        public SearchManager(string vmtype, [Optional] HttpContextBase context)
        {
        }

        #region Search Operations

        public async Task<IViewModel> PostModel(IViewModel searchModel)
        {
            if (searchModel.GetType() == typeof(SearchViewModel))
            {
                return await repo.GetBoxInfos((SearchViewModel)searchModel);
            }
            return searchModel;
        }

        public async Task<IViewModel> PostModel(IViewModel searchModel, SearchViewModel model, HttpContextBase httpcontext)
        {
            if (searchModel.GetType() == typeof(PreBoxInfoViewModel))
            {
                PreBoxInfoViewModel pbvm = await repo.GetPBoxInfo(model);
                PreBoxInfoViewModel pbvmx = (PreBoxInfoViewModel) new PackageOperationsManager(httpcontext, typeof(PreBoxInfoViewModel)).PopulateSearchBox(pbvm);
                return pbvmx;
            }
            else if (searchModel.GetType() == typeof(ComBoxInfoViewModel))
            {
                ComBoxInfoViewModel cbvm = await repo.GetCBoxInfo(model);
                ComBoxInfoViewModel cbvmx = (ComBoxInfoViewModel)new PackageOperationsManager(httpcontext, typeof(ComBoxInfoViewModel)).PopulateSearchBox(cbvm);
                return cbvmx;
            }
            else if (searchModel.GetType() == typeof(CommissionRecordVM))
            {
                CommissionRecordVM crvm = await repo.GetComRec(model);
                CommissionRecordVM crvmx = (CommissionRecordVM)new PackageOperationsManager(httpcontext, typeof(CommissionRecordVM)).PopulateSearchBox(crvm);
                return crvmx;
            }
            return searchModel;
        }

        public async Task<IViewModel> GetModel()
        {
            SearchViewModel model = new SearchViewModel();
            return await repo.GetEmptyBox(model);
        }

        #endregion

        #region  IDisposable implementation

        internal bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            disposed = false;
            if (!disposed)
            {
                if (disposing)
                {
                    // Clear all property values if any
                }
                // Indicate that the instance has been disposed.
                disposed = true;
            }
        }

        #endregion
    }
}
using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Models.Interfaces;
using SJIP_LIMMV1.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SJIP_LIMMV1.Manager.Fluent
{
    public class PreBoxFluentImplementation :IPreBoxCRUDFluent, IPreBoxCreateFluent, IPreBoxReadFluent, IPreBoxUpdateFluent, IPreBoxDeleteFluent
    {
        private PreBoxInfoVMRepo repo;
        private HttpContextBase context;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        //Force Fluent Behaviour using private constructor
        private PreBoxFluentImplementation(HttpContextBase context) { this.context = context; }

        #region Fluent Placeholders
        // Base Interface implementation
        public static IPreBoxCRUDFluent PreBox(HttpContextBase context)
        {
            return new PreBoxFluentImplementation(context);
        }
        public ICreateFluent<PreBoxInfoViewModel> Post() { return this; }
        public IReadFluent<PreBoxInfoViewModel> Get() { return this; }
        public IUpdateFluent<PreBoxInfoViewModel> Update() { return this; }
        public IDeleteFluent<PreBoxInfoViewModel> Delete() { return this; }

        // Extension interface implementation
        IPreBoxCreateFluent IPreBoxCRUDFluent.Post() { return this; }
        IPreBoxReadFluent IPreBoxCRUDFluent.Get() { return this; }
        IPreBoxUpdateFluent IPreBoxCRUDFluent.Update() { return this; }
        IPreBoxDeleteFluent IPreBoxCRUDFluent.Delete() { return this; }

        #endregion

        #region Fluent CRUD Implementation
        // CREATE Implementation
        async Task ICreateFluent<PreBoxInfoViewModel>.UsingModel(PreBoxInfoViewModel vm)
        {
            var findLMPD = string.IsNullOrEmpty(vm.lmpdnum) ? null : await GetRepo().GetByString(vm.lmpdnum);
            if (findLMPD != null)
            {
                await GetRepo().Update(vm);
            }
            else
            {
                IDictionary<string, object> orderdetails = new Dictionary<string, object>();
                var username = await new DashboardManager().generateUserName(context);
                orderdetails.Add("checkername", username);
                PackageOperationsManager POM = new PackageOperationsManager(context, typeof(PreBoxInfoViewModel));
                POM.SetDetails(orderdetails);
                vm = (PreBoxInfoViewModel) POM.PopulatePostBox(vm);
                await GetRepo().Create(vm);
            }
        }

        // READ Implementation
        async Task<PreBoxInfoViewModel> IReadFluent<PreBoxInfoViewModel>.New()
        {
            return await Task.Run(() =>
            {
                PreBoxInfoViewModel preBoxVM;
                PackageOperationsManager POM = new PackageOperationsManager(context, typeof(PreBoxInfoViewModel));
                preBoxVM = (PreBoxInfoViewModel)POM.PopulateNewBox();
                return preBoxVM;
            });
        }

        async Task<PreBoxInfoViewModel> IReadFluent<PreBoxInfoViewModel>.ByID(int id)
        {
            var model = await GetRepo().GetByID(id);
            return (PreBoxInfoViewModel) model;
        }

        async Task<PreBoxInfoViewModel> IPreBoxReadFluent.ByString(string lmpd)
        {
            var model = await GetRepo().GetByString(lmpd);
            return (PreBoxInfoViewModel)model;
        }

        async Task<IEnumerable<PreBoxInfoViewModel>> IReadFluent<PreBoxInfoViewModel>.GetAll()
        {
            return await Task.Run(async () =>
            {
                IEnumerable<IViewModel> models = await GetRepo().GetAll();
                IEnumerable<PreBoxInfoViewModel> returnmodels = models.Cast<PreBoxInfoViewModel>().ToList();
                return returnmodels;
            });
        }

        // Update Implementation
        async Task IUpdateFluent<PreBoxInfoViewModel>.UsingModel(PreBoxInfoViewModel vm)
        {
            if (GetRepo().PreBoxInfoExists(vm.Id))
            {
                await GetRepo().Update(vm);
            }
            else
            {
                logger.Info("Attempt to update PreBoxVM failed as there is no such VM in database");
            }
        }
        
        // Delete Implementation
        async Task IDeleteFluent<PreBoxInfoViewModel>.UsingID(int id)
        {
            if (GetRepo().PreBoxInfoExists(id))
            {
                await GetRepo().Delete(id);
            }
            else
            {
                logger.Info("Attempt to delete PreBoxVM id(" + id + ") failed as there is no such ID in database");
            }
        }

        #endregion

        #region Helpers

        // Accessing Repository
        private PreBoxInfoVMRepo GetRepo()
        {
            if(repo == null)
            {
                repo = new PreBoxInfoVMRepo();
                return repo;
            }
            return repo;
        }

        // Suppressing auto-suggest for Fluency - if ever going external API
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #pragma warning disable 0108
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Type GetType()
        {
            return base.GetType();
        }
        #pragma warning restore 0108

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }
}
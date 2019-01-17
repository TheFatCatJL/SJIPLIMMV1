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
    public class ComBoxFluentImplementation : IComBoxCRUDFluent, IComBoxCreateFluent, IComBoxReadFluent, IComBoxUpdateFluent, IComBoxDeleteFluent
    {
        private ComBoxInfoVMRepo repo;
        private HttpContextBase context;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Force Fluent Behaviour
        private ComBoxFluentImplementation(HttpContextBase context) { this.context = context; }

        #region Fluent Placeholders
        public static IComBoxCRUDFluent ComBox(HttpContextBase context)
        {
            return new ComBoxFluentImplementation(context);
        }

        // Base Interface implementation
        public ICreateFluent<ComBoxInfoViewModel> Post() { return this; }
        public IReadFluent<ComBoxInfoViewModel> Get() { return this; }
        public IUpdateFluent<ComBoxInfoViewModel> Update() { return this; }
        public IDeleteFluent<ComBoxInfoViewModel> Delete() { return this; }

        // Extension interface implementation
        IComBoxReadFluent IComBoxCRUDFluent.Get() { return this; }
        IComBoxCreateFluent IComBoxCRUDFluent.Post() { return this; }
        IComBoxDeleteFluent IComBoxCRUDFluent.Delete() { return this; }
        IComBoxUpdateFluent IComBoxCRUDFluent.Update() { return this; }

        #endregion

        #region Fluent CRUD Implementation
        // CREATE Implementation
        async Task ICreateFluent<ComBoxInfoViewModel>.UsingModel(ComBoxInfoViewModel vm)
        {
            if (GetRepo().ComBoxInfoExists(vm.Id))
            {
                await GetRepo().Update(vm);
            }
            else
            {
                IDictionary<string, object> orderdetails = new Dictionary<string, object>();
                var username = await new DashboardManager().generateUserName(context);
                orderdetails.Add("supname", username);
                PackageOperationsManager POM = new PackageOperationsManager(context, typeof(ComBoxInfoViewModel));
                POM.SetDetails(orderdetails);
                vm = (ComBoxInfoViewModel) POM.PopulatePostBox(vm);
                if (vm.comrec == null)
                {
                    vm.comrec = new CommissionRecordVM();
                    vm.comrec.supname = username;
                    vm.comrec.status = "Report Pending";
                    vm.comrec.comrecorddate = (DateTime)vm.rptcomdate;
                }
                await GetRepo().Create(vm);
            }
        }

        // READ Implementation
        async Task<ComBoxInfoViewModel> IReadFluent<ComBoxInfoViewModel>.New()
        {
            return await Task.Run(() =>
            {
                PackageOperationsManager POM = new PackageOperationsManager(context, typeof(ComBoxInfoViewModel));
                ComBoxInfoViewModel ComBoxVM = (ComBoxInfoViewModel)POM.PopulateNewBox();
                return ComBoxVM;
            });
        }

        async Task<ComBoxInfoViewModel> IReadFluent<ComBoxInfoViewModel>.ByID(int id)
        {
            var model = await GetRepo().GetByID(id);
            return (ComBoxInfoViewModel)model;
        }

        async Task<ComBoxInfoViewModel> IComBoxReadFluent.ByString(string teamname)
        {
            var model = await GetRepo().GetByString(teamname);
            return (ComBoxInfoViewModel)model;
        }

        async Task<IEnumerable<ComBoxInfoViewModel>> IComBoxReadFluent.ByComRec(int comrecnum)
        {
            IEnumerable<IViewModel> models = await GetRepo().GetAll();
            IEnumerable<ComBoxInfoViewModel> returnmodels = models.Cast<ComBoxInfoViewModel>().Where(x => x.comrec.Id== comrecnum).ToList();
            return returnmodels;
        }

        async Task<IEnumerable<ComBoxInfoViewModel>> IReadFluent<ComBoxInfoViewModel>.GetAll()
        {
            IEnumerable<IViewModel> models = await GetRepo().GetAll();
            IEnumerable<ComBoxInfoViewModel> returnmodels = models.Cast<ComBoxInfoViewModel>().ToList();
            return returnmodels;
        }

        // Update Implementation
        async Task IUpdateFluent<ComBoxInfoViewModel>.UsingModel(ComBoxInfoViewModel vm)
        {
            if (GetRepo().ComBoxInfoExists(vm.Id))
            {
                await GetRepo().Update(vm);
            }
            else
            {
                logger.Info("Attempt to update ComBoxVM failed as there is no such VM in database");
            }
        }


        // Delete Implementation
        async Task IDeleteFluent<ComBoxInfoViewModel>.UsingID(int id)
        {
            if (GetRepo().ComBoxInfoExists(id))
            {
                await GetRepo().Delete(id);
            }
            else
            {
                logger.Info("Attempt to delete ComBoxVM id(" + id + ") failed as there is no such ID in database");
            }
        }

        #endregion

        #region Helpers

        // Accessing Repository
        private ComBoxInfoVMRepo GetRepo()
        {
            if(repo == null)
            {
                repo = new ComBoxInfoVMRepo();
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
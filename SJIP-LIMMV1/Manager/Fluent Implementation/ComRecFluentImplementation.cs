using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Models.Interfaces;
using SJIP_LIMMV1.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace SJIP_LIMMV1.Manager.Fluent
{
    public class ComRecFluentImplementation : IComRecCRUDFluent, IComRecCreateFluent, IComRecReadFluent, IComRecUpdateFluent, IComRecDeleteFluent
    {
        private CommissionRecordVMRepo repo;
        private HttpContextBase context;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Force Fluent Behaviour
        private ComRecFluentImplementation(HttpContextBase context) { this.context = context; }

        #region Fluent Placeholders
        public static IComRecCRUDFluent ComRec(HttpContextBase context)
        {
            return new ComRecFluentImplementation(context);
        }

        // Base Interface implementation
        public ICreateFluent<CommissionRecordVM> Post() { return this; }
        public IReadFluent<CommissionRecordVM> Get() { return this; }
        public IUpdateFluent<CommissionRecordVM> Update() { return this; }
        public IDeleteFluent<CommissionRecordVM> Delete() { return this; }

        // Extension interface implementation
        IComRecReadFluent IComRecCRUDFluent.Get() { return this; }
        IComRecCreateFluent IComRecCRUDFluent.Post() { return this; }
        IComRecDeleteFluent IComRecCRUDFluent.Delete() { return this; }
        IComRecUpdateFluent IComRecCRUDFluent.Update() { return this; }

        #endregion

        #region Fluent CRUD Implementation
        // CREATE Implementation
        async Task ICreateFluent<CommissionRecordVM>.UsingModel(CommissionRecordVM vm)
        {
            vm.status = "Commission Record Submitted";
            if (GetRepo().CommissionRecordExists(vm.Id))
            {
                await GetRepo().Update(vm);
            }
            else
            {
                PackageOperationsManager POM = new PackageOperationsManager(context, typeof(CommissionRecordVM));
                CommissionRecordVM cr = (CommissionRecordVM)POM.PopulatePostBox(vm);
                await GetRepo().Create(cr);
            }
            foreach (var combox in vm.vms)
            {
                PreBoxInfoViewModel pbvm = await PreBoxFluentImplementation.PreBox(context).Get().ByString(combox.lmpdnum);
                pbvm.status = "Commissioned";
                await PreBoxFluentImplementation.PreBox(context).Update().UsingModel(pbvm);
            }
        }

        // READ Implementation
        async Task<CommissionRecordVM> IReadFluent<CommissionRecordVM>.New()
        {
            return await Task.Run(() =>
            {
                PackageOperationsManager POM = new PackageOperationsManager(context, typeof(CommissionRecordVM));
                IDictionary<string, object> orderdetails = new Dictionary<string, object>();
                var username = new DashboardManager().generateUserName(context);
                orderdetails.Add("supname", username.Result);
                POM.SetDetails(orderdetails);
                CommissionRecordVM ComRecVM = (CommissionRecordVM)POM.PopulateNewBox();
                return ComRecVM;
            });
        }

        async Task<CommissionRecordVM> IReadFluent<CommissionRecordVM>.ByID(int id)
        {
            var model = await GetRepo().GetByID(id);
            return (CommissionRecordVM)model;
        }

        async Task<CommissionRecordVM> IComRecReadFluent.ByUser(IIdentity user)
        {
            var model = await GetRepo().GetByUser(user);
            return (CommissionRecordVM)model;
        }

        async Task<CommissionRecordVM> IComRecReadFluent.ByLMPD(string lmpd)
        {
            var model = await GetRepo().GetByLMPD(lmpd);
            return (CommissionRecordVM)model;
        }


        async Task<IEnumerable<CommissionRecordVM>> IReadFluent<CommissionRecordVM>.GetAll()
        {
            var models = await GetRepo().GetAll();
            return models.Cast<CommissionRecordVM>().ToList();
        }

        // Update Implementation
        async Task IUpdateFluent<CommissionRecordVM>.UsingModel(CommissionRecordVM vm)
        {
            if (GetRepo().CommissionRecordExists(vm.Id))
            {
                await GetRepo().Update(vm);
            }
            else
            {
                logger.Info("Attempt to update CommissionRecord failed as there is no such VM in database");
            }
        }


        // Delete Implementation
        async Task IDeleteFluent<CommissionRecordVM>.UsingID(int id)
        {
            if (GetRepo().CommissionRecordExists(id))
            {
                await GetRepo().Delete(id);
            }
            else
            {
                logger.Info("Attempt to delete CommissionRecord id(" + id + ") failed as there is no such ID in database");
            }
        }

        #endregion

        #region Helpers

        // Accessing Repository
        private CommissionRecordVMRepo GetRepo()
        {
            if(repo == null)
            {
                repo = new CommissionRecordVMRepo();
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
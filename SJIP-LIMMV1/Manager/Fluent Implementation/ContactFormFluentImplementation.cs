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
    public class ContactFormFluentImplementation : IContactFormCRUDFluent, IContactFormCreateFluent, IContactFormReadFluent, IContactFormUpdateFluent, IContactFormDeleteFluent
    {
        private ContactFormVMRepo repo;
        private HttpContextBase context;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Force Fluent Behaviour
        private ContactFormFluentImplementation(HttpContextBase context) { this.context = context; }

        #region Fluent Placeholders
        public static IContactFormCRUDFluent ContactForm(HttpContextBase context)
        {
            return new ContactFormFluentImplementation(context);
        }

        // Base Interface implementation
        public ICreateFluent<ContactFormViewModel> Post() { return this; }
        public IReadFluent<ContactFormViewModel> Get() { return this; }
        public IUpdateFluent<ContactFormViewModel> Update() { return this; }
        public IDeleteFluent<ContactFormViewModel> Delete() { return this; }

        // Extension interface implementation
        IContactFormReadFluent IContactFormCRUDFluent.Get() { return this; }
        IContactFormCreateFluent IContactFormCRUDFluent.Post() { return this; }
        IContactFormDeleteFluent IContactFormCRUDFluent.Delete() { return this; }
        IContactFormUpdateFluent IContactFormCRUDFluent.Update() { return this; }

        #endregion

        #region Fluent CRUD Implementation
        // CREATE Implementation
        async Task ICreateFluent<ContactFormViewModel>.UsingModel(ContactFormViewModel vm)
        {
            if (GetRepo().ContactFormExists(vm.Id))
            {
                await GetRepo().Update(vm);
            }
            else
            {
                ContactFormViewModel ConFormVM = (ContactFormViewModel)vm;
                PackageOperationsManager POM = new PackageOperationsManager(context, typeof(ContactFormViewModel));
                ConFormVM = (ContactFormViewModel) POM.PopulatePostBox(ConFormVM);
                await GetRepo().Create(ConFormVM);
            }
        }

        // READ Implementation
        async Task<ContactFormViewModel> IReadFluent<ContactFormViewModel>.New()
        {
            return await Task.Run(() =>
            {
                IDictionary<string, object> orderdetails = new Dictionary<string, object>();
                PackageOperationsManager POM = new PackageOperationsManager(context, typeof(ContactFormViewModel));
                var username = new DashboardManager().generateUserName(context);
                var email = new DashboardManager().generateUserEmail(context);
                orderdetails.Add("username", username.Result);
                orderdetails.Add("email", email.Result);
                POM.SetDetails(orderdetails);
                ContactFormViewModel contactFormVM = (ContactFormViewModel)POM.PopulateNewBox();
                return contactFormVM;
            });
        }

        async Task<ContactFormViewModel> IReadFluent<ContactFormViewModel>.ByID(int id)
        {
            var model = await GetRepo().GetByID(id);
            return (ContactFormViewModel)model;
        }

        async Task<ContactFormViewModel> IContactFormReadFluent.ByString(string email)
        {
            var model = await GetRepo().GetByString(email);
            return (ContactFormViewModel)model;
        }

        async Task<IEnumerable<ContactFormViewModel>> IReadFluent<ContactFormViewModel>.GetAll()
        {
            var models = await GetRepo().GetAll();
            return models.Cast<ContactFormViewModel>().ToList();
        }

        // Update Implementation
        async Task IUpdateFluent<ContactFormViewModel>.UsingModel(ContactFormViewModel vm)
        {
            if (GetRepo().ContactFormExists(vm.Id))
            {
                await GetRepo().Update(vm);
            }
            else
            {
                logger.Info("Attempt to update ContactFormVM failed as there is no such VM in database");
            }
        }


        // Delete Implementation
        async Task IDeleteFluent<ContactFormViewModel>.UsingID(int id)
        {
            if (GetRepo().ContactFormExists(id))
            {
                await GetRepo().Delete(id);
            }
            else
            {
                logger.Info("Attempt to delete ContactFormVM id(" + id + ") failed as there is no such ID in database");
            }
        }

        #endregion

        #region Helpers

        // Accessing Repository
        private ContactFormVMRepo GetRepo()
        {
            if(repo == null)
            {
                repo = new ContactFormVMRepo();
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
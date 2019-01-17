using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SJIP_LIMMV1.Manager.Fluent;
using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Models.Interfaces;

namespace SJIP_LIMMV1.Manager
{
    // This class acts as the intermediary class for client view-controller and controller-logic layer
    // It is designed to be eventually exposed as the API interface with an additional layer of fluent interface to support fluent API style calls.
    public class ViewModelManager <T>
        where T : IViewModel
    {
        private HttpContextBase context { get; set; }
        public ViewModelManager(HttpContextBase context)
        {
            this.context = context;
        }

        #region Get ViewModel/ViewModels
        public async Task<T> GetNew()
        {
            IViewModel basemodel;
            if (typeof(T) == typeof(PreBoxInfoViewModel))
                basemodel = await PreBoxFluentImplementation.PreBox(context).Get().New();
            else if (typeof(T) == typeof(ComBoxInfoViewModel))
                basemodel = await ComBoxFluentImplementation.ComBox(context).Get().New();
            else if (typeof(T) == typeof(CommissionRecordVM))
                basemodel = await ComRecFluentImplementation.ComRec(context).Get().New();
            else if (typeof(T) == typeof(ContactFormViewModel))
                basemodel = await ContactFormFluentImplementation.ContactForm(context).Get().New();
            else
                return default(T);
            return (T)basemodel;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            IEnumerable<IViewModel> basemodels;
            if (typeof(T) == typeof(PreBoxInfoViewModel))
                basemodels = await PreBoxFluentImplementation.PreBox(context).Get().GetAll();
            else if (typeof(T) == typeof(ComBoxInfoViewModel))
                basemodels = await ComBoxFluentImplementation.ComBox(context).Get().GetAll();
            else if (typeof(T) == typeof(CommissionRecordVM))
                basemodels = await ComRecFluentImplementation.ComRec(context).Get().GetAll();
            else if (typeof(T) == typeof(ContactFormViewModel))
                basemodels = await ContactFormFluentImplementation.ContactForm(context).Get().GetAll();
            else
                return default(IEnumerable<T>);
            return basemodels.Cast<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetBySpecial(int specialparam)
        {
            IEnumerable<IViewModel> basemodels;
            if (typeof(T) == typeof(ComBoxInfoViewModel))
                basemodels = await ComBoxFluentImplementation.ComBox(context).Get().ByComRec(specialparam);
            else
                return default(IEnumerable<T>);
            return basemodels.Cast<T>().ToList();
        }

        public async Task<T> GetOneBySpecial(string specialparam)
        {
            IViewModel basemodel;
            if (typeof(T) == typeof(PreBoxInfoViewModel))
                basemodel = await PreBoxFluentImplementation.PreBox(context).Get().ByString(specialparam);
            else if (typeof(T) == typeof(CommissionRecordVM))
                basemodel = await ComRecFluentImplementation.ComRec(context).Get().ByLMPD(specialparam);
            else
                return default(T);
            return (T)basemodel;
        }

        public async Task<T> GetByID(int id)
        {
            IViewModel basemodel;
            if (id > 0)
            {
                if (typeof(T) == typeof(PreBoxInfoViewModel))
                    basemodel = await PreBoxFluentImplementation.PreBox(context).Get().ByID(id);
                else if (typeof(T) == typeof(ComBoxInfoViewModel))
                    basemodel = await ComBoxFluentImplementation.ComBox(context).Get().ByID(id);
                else if (typeof(T) == typeof(CommissionRecordVM))
                    basemodel = await ComRecFluentImplementation.ComRec(context).Get().ByID(id);
                else if (typeof(T) == typeof(ContactFormViewModel))
                    basemodel = await ContactFormFluentImplementation.ContactForm(context).Get().ByID(id);
                else
                    return default(T);
                return (T)basemodel;
            }
            return default(T);
        }
        #endregion

        #region Post ViewModel
        public async Task Post(T vm)
        {
            if (typeof(T) == typeof(PreBoxInfoViewModel))
                await PreBoxFluentImplementation.PreBox(context).Post().UsingModel((PreBoxInfoViewModel)(IViewModel)vm);
            else if (typeof(T) == typeof(ComBoxInfoViewModel))
                await ComBoxFluentImplementation.ComBox(context).Post().UsingModel((ComBoxInfoViewModel)(IViewModel)vm);
            else if (typeof(T) == typeof(CommissionRecordVM))
                await ComRecFluentImplementation.ComRec(context).Post().UsingModel((CommissionRecordVM)(IViewModel)vm);
            else if (typeof(T) == typeof(ContactFormViewModel))
                await ContactFormFluentImplementation.ContactForm(context).Post().UsingModel((ContactFormViewModel)(IViewModel)vm);
        }

        #endregion

        #region Delete ViewModel
        public async Task Delete(int id)
        {
            if (typeof(T) == typeof(PreBoxInfoViewModel))
                await PreBoxFluentImplementation.PreBox(context).Delete().UsingID(id);
            else if (typeof(T) == typeof(ComBoxInfoViewModel))
                await ComBoxFluentImplementation.ComBox(context).Delete().UsingID(id);
            else if (typeof(T) == typeof(CommissionRecordVM))
                await ComRecFluentImplementation.ComRec(context).Delete().UsingID(id);
            else if (typeof(T) == typeof(ContactFormViewModel))
                await ContactFormFluentImplementation.ContactForm(context).Delete().UsingID(id);
        }

        #endregion

    }
}
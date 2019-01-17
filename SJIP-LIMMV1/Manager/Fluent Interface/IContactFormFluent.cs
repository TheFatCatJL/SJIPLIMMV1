using System.Collections.Generic;
using System.Threading.Tasks;
using SJIP_LIMMV1.Models.Interfaces;
using SJIP_LIMMV1.Models;

namespace SJIP_LIMMV1.Manager.Fluent
{
    #region FLUENT CRUD IMPLEMENTATION
    // FLUENT CRUD
    public interface IContactFormCRUDFluent : IFluent<ContactFormViewModel>
    {
        // Generic CRUD
        new IContactFormReadFluent Get();
        new IContactFormCreateFluent Post();
        new IContactFormDeleteFluent Delete();
        new IContactFormUpdateFluent Update();
    }

    // CREATE FLUENT
    public interface IContactFormCreateFluent : ICreateFluent<ContactFormViewModel> { }

    // READ FLUENT
    public interface IContactFormReadFluent : IReadFluent<ContactFormViewModel>
    {
        Task<ContactFormViewModel> ByString(string email);
    }

    // UPDATE FLUENT
    public interface IContactFormUpdateFluent : IUpdateFluent<ContactFormViewModel> { }

    // DELETE FLUENT
    public interface IContactFormDeleteFluent : IDeleteFluent<ContactFormViewModel> { }

    #endregion
}

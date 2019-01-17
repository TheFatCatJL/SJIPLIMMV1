using System.Collections.Generic;
using System.Threading.Tasks;
using SJIP_LIMMV1.Models.Interfaces;
using SJIP_LIMMV1.Models;

namespace SJIP_LIMMV1.Manager.Fluent
{

    #region FLUENT CRUD IMPLEMENTATION
    // FLUENT CRUD
    public interface IComBoxCRUDFluent : IFluent<ComBoxInfoViewModel>
    {
        // Generic CRUD
        new IComBoxReadFluent Get();
        new IComBoxCreateFluent Post();
        new IComBoxDeleteFluent Delete();
        new IComBoxUpdateFluent Update();
    }

    // CREATE FLUENT
    public interface IComBoxCreateFluent : ICreateFluent<ComBoxInfoViewModel> { }

    // READ FLUENT
    public interface IComBoxReadFluent : IReadFluent<ComBoxInfoViewModel>
    {
        Task<ComBoxInfoViewModel> ByString(string teamname);
        Task<IEnumerable<ComBoxInfoViewModel>> ByComRec(int comrecnum);
    }

    // UPDATE FLUENT
    public interface IComBoxUpdateFluent : IUpdateFluent<ComBoxInfoViewModel> { }

    // DELETE FLUENT
    public interface IComBoxDeleteFluent : IDeleteFluent<ComBoxInfoViewModel> { }

    #endregion
}

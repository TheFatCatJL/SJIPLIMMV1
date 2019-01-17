using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace SJIP_LIMMV1.Manager.Fluent
{
    #region PreBox FLUENT CRUD IMPLEMENTATION
    // FLUENT CRUD
    public interface IPreBoxCRUDFluent : IFluent<PreBoxInfoViewModel>
    { 
        new IPreBoxCreateFluent Post();
        new IPreBoxReadFluent Get();
        new IPreBoxUpdateFluent Update();
        new IPreBoxDeleteFluent Delete();
    }

    // CREATE FLUENT
    public interface IPreBoxCreateFluent : ICreateFluent<PreBoxInfoViewModel> { }

    // READ FLUENT
    public interface IPreBoxReadFluent : IReadFluent<PreBoxInfoViewModel>
    {
        Task<PreBoxInfoViewModel> ByString(string lmpd);
    }

    // UPDATE FLUENT
    public interface IPreBoxUpdateFluent : IUpdateFluent<PreBoxInfoViewModel> { }

    // DELETE FLUENT
    public interface IPreBoxDeleteFluent : IDeleteFluent<PreBoxInfoViewModel> { }

    #endregion
}

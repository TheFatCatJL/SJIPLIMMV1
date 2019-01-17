using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using SJIP_LIMMV1.Models.Interfaces;
using SJIP_LIMMV1.Models;

namespace SJIP_LIMMV1.Manager.Fluent
{
    #region FLUENT CRUD IMPLEMENTATION
    // FLUENT CRUD
    public interface IComRecCRUDFluent : IFluent<CommissionRecordVM>
    {
        // Generic CRUD
        new IComRecReadFluent Get();
        new IComRecCreateFluent Post();
        new IComRecDeleteFluent Delete();
        new IComRecUpdateFluent Update();
    }

    // CREATE FLUENT
    public interface IComRecCreateFluent : ICreateFluent<CommissionRecordVM> { }

    // READ FLUENT
    public interface IComRecReadFluent : IReadFluent<CommissionRecordVM>
    {
        Task<CommissionRecordVM> ByUser(IIdentity user);
        Task<CommissionRecordVM> ByLMPD(string LMPD);
    }

    // UPDATE FLUENT
    public interface IComRecUpdateFluent : IUpdateFluent<CommissionRecordVM> { }

    // DELETE FLUENT
    public interface IComRecDeleteFluent : IDeleteFluent<CommissionRecordVM> { }

    #endregion
}

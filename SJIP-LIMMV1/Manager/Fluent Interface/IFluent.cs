using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using SJIP_LIMMV1.Models.Interfaces;

namespace SJIP_LIMMV1.Manager.Fluent
{

    #region FLUENT CRUD IMPLEMENTATION
    // FLUENT CRUD
    public interface IFluent<T> where T : IViewModel
    {
        // Generic CRUD
        ICreateFluent<T> Post();
        IReadFluent<T> Get();
        IUpdateFluent<T> Update();
        IDeleteFluent<T> Delete();
    }

    // CREATE FLUENT
    public interface ICreateFluent<T>
    {
        Task UsingModel(T vm);
    }

    // READ FLUENT
    public interface IReadFluent<T>
    {
        Task<T> New();
        Task<T> ByID(int id);
        Task<IEnumerable<T>> GetAll();
    }

    // UPDATE FLUENT
    public interface IUpdateFluent<T>
    {
        Task UsingModel(T vm);
    }

    // DELETE FLUENT
    public interface IDeleteFluent<T>
    {
        Task UsingID(int id);
    }
    #endregion
}

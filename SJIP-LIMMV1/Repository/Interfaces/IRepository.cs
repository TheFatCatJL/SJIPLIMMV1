using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SJIP_LIMMV1.Repository
{
    public interface IRepository<T> : IDisposable
    {
        #region Read Operations

        // Get using ID
        Task<T> GetByID(int id);
        // Get collection of VM
        Task<IEnumerable<T>> GetAll();

        #endregion

        #region Create Operations

        // Create (Form submission)
        Task Create(T vm);

        #endregion

        #region Edit / Delete Operations

        // Edit (Form Submission)
        Task Update(T vm);
        // Delete (Base on ID)
        Task Delete(int id);

        #endregion

    }
}
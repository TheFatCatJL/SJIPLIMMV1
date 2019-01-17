using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using SJIP_LIMMV1.Models;
using System.Threading.Tasks;
using SJIP_LIMMV1.Models.Interfaces;
using AutoMapper;

namespace SJIP_LIMMV1.Repository
{
    public class PreBoxInfoVMRepo : IRepository<IViewModel>
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BoxModelEntities db = new BoxModelEntities();

        #region Get Operations
        // Get using ID
        public async Task<IViewModel> GetByID(int id)
        {
            try
            {
                var requestVM = await db.PreBoxInfoes.FirstOrDefaultAsync(x => x.preboxId == id);
                if (requestVM == null)
                {
                    logger.Info("BoxInfoVM of ID : " + id + " not found.");
                    return null;
                }
                logger.Info("BoxInfoVM of ID : " + id + " found.");
                PreBoxInfoViewModel pbvm = Mapper.Map<PreBoxInfo, PreBoxInfoViewModel>(requestVM);
                return pbvm;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return null;
            }
        }

        // Get using LMPD
        public async Task<IViewModel> GetByString(string lmpd)
        {
            try
            {
                var requestVM = await db.PreBoxInfoes.FirstOrDefaultAsync(x => x.lmpdnum == lmpd);
                if (requestVM == null)
                {
                    logger.Info("BoxInfoVM of LMPD : " + lmpd + " not found.");
                    return null;
                }
                logger.Info("BoxInfoVM of LMPD : " + lmpd + " found.");
                PreBoxInfoViewModel pbvm = Mapper.Map<PreBoxInfo, PreBoxInfoViewModel>(requestVM);
                return pbvm;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return null;
            }
        }
        
        // GET all
        public async Task<IEnumerable<IViewModel>> GetAll()
        {
            try
            {
                var requestVMList = await db.PreBoxInfoes.ToListAsync();
                if (requestVMList == null)
                {
                    logger.Info("BoxInfoVMList is empty. Either there is no data or there is unknown error");
                    return null;
                }
                logger.Info("BoxInfoVMList found and returned.");
                IEnumerable<PreBoxInfoViewModel> pbvms = Mapper.Map<List<PreBoxInfo>, IEnumerable<PreBoxInfoViewModel>>(requestVMList);
                return pbvms;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return null;
            }
        }

        #endregion

        #region Put & Post Operations
        //Edit
        public async Task Update(IViewModel vm)
        {
            // Retrieve entity base on the LMPD number (which is technically unique and should not double entry)
            try
            {
                PreBoxInfoViewModel pvm = (PreBoxInfoViewModel)vm;
                PreBoxInfo pbi = Mapper.Map<PreBoxInfoViewModel, PreBoxInfo>(pvm);
                PreBoxInfo entity = await db.PreBoxInfoes.FirstOrDefaultAsync(x => x.lmpdnum == pvm.lmpdnum);
                if (entity == null)
                {
                    logger.Info("There is no record in database of the BoxInfoVM ID provided (" + vm.Id + ")");
                }
                db.Entry(entity).CurrentValues.SetValues(pbi);
                await db.SaveChangesAsync();
                logger.Info("Records Updated for BoxInfoVM ID (" + vm.Id + ")");
            }
            catch (DbUpdateConcurrencyException exception)
            {
                RefreshDBContextForConcurrencyClientWin();
                if (!PreBoxInfoExists(vm.Id))
                    logger.Error("An attempted to save BoxInfoVM(" + vm.Id + ") failed due to concurrency. Error Trace :" + exception);
                else
                    logger.Error("An attempted to save BoxInfoVM(" + vm.Id + ") failed as VM no longer exists. Error Trace :" + exception);
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
            }
        }

        //Submit
        public async Task Create(IViewModel vm)
        {
            try
            {
                PreBoxInfoViewModel pvm = (PreBoxInfoViewModel)vm;
                PreBoxInfo pbi = Mapper.Map<PreBoxInfoViewModel, PreBoxInfo>(pvm);
                db.PreBoxInfoes.Add(pbi);
                await db.SaveChangesAsync();
                logger.Info("Posting of BoxInfoVM Successful. " + pvm.ToString());
            }
            catch (DbUpdateConcurrencyException e)
            {
                RefreshDBContextForConcurrencyClientWin();
                logger.Error("An attempted to save BoxInfoVM failed due to concurrency. Error Trace :" + e);
            }
            catch (Exception e)
            {
                logger.Error("Error when trying to create a BoxInfo entry in database. Error Trace : " + e);
            }
        }

        #endregion

        #region Delete Operations
        //Delete
        public async Task Delete(int id)
        {
            try
            {
                PreBoxInfo pbi = await db.PreBoxInfoes.FindAsync(id);
                if (pbi != null)
                {
                    db.PreBoxInfoes.Remove(pbi);
                    await db.SaveChangesAsync();
                    logger.Info("Deletion of BoxInfoVM id(" + id + ") Successful.");
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                RefreshDBContextForConcurrencyStoreWin();
                logger.Error("Attempt to delete BoxInfoVM id(" + id + ") failed due to concurrency. Error Trace :" + e);
            }
            catch (Exception e)
            {
                logger.Error("Error when trying to delete BoxInfoVM id(" + id + ") from database. Error Trace : " + e);
            }
        }

        #endregion
        
        #region Helpers

        // Simple attempt to resolve Concurrency Issue - Client wins
        private void RefreshDBContextForConcurrencyClientWin()
        {
            var context = ((IObjectContextAdapter)db).ObjectContext;
            var refreshableObjects = db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
            context.Refresh(System.Data.Entity.Core.Objects.RefreshMode.ClientWins, refreshableObjects);
        }

        // Simple attempt to resolve Concurrency Issue - Store wins
        private void RefreshDBContextForConcurrencyStoreWin()
        {
            var context = ((IObjectContextAdapter)db).ObjectContext;
            var refreshableObjects = db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
            context.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, refreshableObjects);
        }

        public bool PreBoxInfoExists(int id)
        {
            return db.PreBoxInfoes.Count(e => e.preboxId == id) > 0;
        }

        #endregion

        #region  IDisposable implementation

        internal bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            ((IDisposable)db).Dispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            disposed = false;
            if (!disposed)
            {
                if (disposing)
                {
                    // Clear all property values if any
                }
                // Indicate that the instance has been disposed.
                disposed = true;
            }
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using SJIP_LIMMV1.Models;
using System.Threading.Tasks;

namespace SJIP_LIMMV1.Repository
{
    public class BoxInfoRepo : IDisposable
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BoxInfoContext db = new BoxInfoContext();

        #region Get Operations
        // Get using ID
        public async Task<BoxInfoViewModel> GetBoxInfoViewModel(int id)
        {
            try
            {
                var requestVM = await db.BoxInfoViewModels.FirstOrDefaultAsync(x => x.id == id);
                if (requestVM == null)
                {
                    logger.Info("BoxInfoVM of ID : " + id + " not found.");
                    return null;
                }
                logger.Info("BoxInfoVM of ID : " + id + " found.");
                return requestVM;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return null;
            }
        }

        // Get using LMPD
        public async Task<BoxInfoViewModel> GetBoxInfoViewModel(string lmpd)
        {
            try
            {
                var requestVM = await db.BoxInfoViewModels.FirstOrDefaultAsync(x => x.lmpdnum == lmpd);
                if (requestVM == null)
                {
                    logger.Info("BoxInfoVM of LMPD : " + lmpd + " not found.");
                    return null;
                }
                logger.Info("BoxInfoVM of LMPD : " + lmpd + " found.");
                return requestVM;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return null;
            }
        }


        // GETs
        public async Task<IEnumerable<BoxInfoViewModel>> GetBoxInfoViewModels()
        {
            try
            {
                var requestVMList = await db.BoxInfoViewModels.ToListAsync();
                if (requestVMList == null)
                {
                    logger.Info("BoxInfoVMList is empty. Either there is no data or there is unknown error");
                    return null;
                }
                logger.Info("BoxInfoVMList found and returned.");
                return requestVMList;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return null;
            }
        }

        #endregion

        #region Post/Put Operations
        //Edit
        public async Task PutBoxInfoViewModel(BoxInfoViewModel boxInfoViewModel)
        {
            // Retrieve entity base on the LMPD number (which is technically unique and should not double entry)
            try
            {
                BoxInfoViewModel entity = await db.BoxInfoViewModels.FirstOrDefaultAsync(x => x.lmpdnum == boxInfoViewModel.lmpdnum);
                if (entity == null)
                {
                    logger.Info("There is no record in database of the BoxInfoVM ID provided (" + boxInfoViewModel.id + ")");
                }
                db.Entry(entity).CurrentValues.SetValues(boxInfoViewModel);
                await db.SaveChangesAsync();
                logger.Info("Records Updated for BoxInfoVM ID (" + boxInfoViewModel.id + ")");
            } 
            catch (DbUpdateConcurrencyException exception)
            {                
                RefreshDBContextForConcurrencyClientWin();
                if (!BoxInfoViewModelExists(boxInfoViewModel.id))
                    logger.Error("An attempted to save BoxInfoVM(" + boxInfoViewModel.id + ") failed due to concurrency. Error Trace :" + exception);
                else
                    logger.Error("An attempted to save BoxInfoVM(" + boxInfoViewModel.id + ") failed as VM no longer exists. Error Trace :" + exception);
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
            }
        }

        //Submit
        public async Task PostBoxInfoViewModel(BoxInfoViewModel boxInfoViewModel)
        {
            try
            {
                db.BoxInfoViewModels.Add(boxInfoViewModel);
                await db.SaveChangesAsync();
                logger.Info("Posting of BoxInfoVM Successful. " + boxInfoViewModel.ToString());
            }
            catch (DbUpdateConcurrencyException e)
            {
                RefreshDBContextForConcurrencyClientWin();
                logger.Error("An attempted to save BoxInfoVM failed due to concurrency. " + boxInfoViewModel.ToString() + "Error Trace :" + e);
            }
            catch (Exception e)
            {
                logger.Error("Error when trying to create a BoxInfo entry in database. " + boxInfoViewModel.ToString() + "Error Trace : " + e);
            }
        }

        #endregion
        
        #region Delete Operations
        //Delete
        public async Task DeleteBoxInfoViewModel(int id)
        {
            try
            {
                BoxInfoViewModel boxInfoViewModel = await db.BoxInfoViewModels.FindAsync(id);
                if (boxInfoViewModel != null)
                {
                    db.BoxInfoViewModels.Remove(boxInfoViewModel);
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

        public bool BoxInfoViewModelExists(int id)
        {
            return db.BoxInfoViewModels.Count(e => e.id == id) > 0;
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
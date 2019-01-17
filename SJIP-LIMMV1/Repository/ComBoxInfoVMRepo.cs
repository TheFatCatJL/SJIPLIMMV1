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
    public class ComBoxInfoVMRepo : IRepository<IViewModel>
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BoxModelEntities db = new BoxModelEntities();

        #region Get Operations
        // Get using ID
        public async Task<IViewModel> GetByID(int id)
        {
            try
            {
                var requestVM = await db.ComBoxInfoes.FirstOrDefaultAsync(x => x.comboxId == id);
                if (requestVM == null)
                {
                    logger.Info("ComBoxInfo of ID : " + id + " not found.");
                    return null;
                }
                logger.Info("ComBoxInfoVof ID : " + id + " found.");
                ComBoxInfoViewModel cbvm = Mapper.Map<ComBoxInfo, ComBoxInfoViewModel>(requestVM);
                return cbvm;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return null;
            }
        }

        // Get using LMPD
        public async Task<IViewModel> GetByString(string teamname)
        {
            try
            {
                var requestVM = await db.ComBoxInfoes.FirstOrDefaultAsync(x => x.teamname == teamname);
                if (requestVM == null)
                {
                    logger.Info("ComBoxInfoVM not found.");
                    return null;
                }
                logger.Info("ComBoxInfoVM found.");
                ComBoxInfoViewModel cbvm = Mapper.Map<ComBoxInfo, ComBoxInfoViewModel>(requestVM);
                return cbvm;
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
                var requestVMList = await db.ComBoxInfoes.ToListAsync();
                if (requestVMList == null)
                {
                    logger.Info("BoxInfoVMList is empty. Either there is no data or there is unknown error");
                    return null;
                }
                logger.Info("BoxInfoVMList found and returned.");
                List<ComBoxInfoViewModel> cbvms = Mapper.Map<List<ComBoxInfo>, List<ComBoxInfoViewModel>>(requestVMList);
                return cbvms;
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
            // Retrieve entity base on ID
            try
            {
                ComBoxInfoViewModel comBoxVM = (ComBoxInfoViewModel)vm;
                ComBoxInfo cbi = Mapper.Map<ComBoxInfoViewModel, ComBoxInfo>(comBoxVM);
                ComBoxInfo entity = await db.ComBoxInfoes.FirstOrDefaultAsync(x => x.comboxId == comBoxVM.Id);
                if (entity == null)
                {
                    logger.Info("There is no record in database of the BoxInfoVM ID provided (" + vm.Id + ")");
                }
                db.Entry(entity).CurrentValues.SetValues(cbi);
                await db.SaveChangesAsync();
                logger.Info("Records Updated for BoxInfoVM ID (" + vm.Id + ")");
            }
            catch (DbUpdateConcurrencyException exception)
            {
                RefreshDBContextForConcurrencyClientWin();
                if (!ComBoxInfoExists(vm.Id))
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
                ComBoxInfoViewModel comBoxVM = (ComBoxInfoViewModel)vm;
                ComBoxInfo cbi = Mapper.Map<ComBoxInfoViewModel, ComBoxInfo>(comBoxVM);
                cbi.Address = db.Addresses.Find(int.Parse(comBoxVM.rptpostalcode));
                cbi.rptdate = DateTime.Now;
                if (comBoxVM.comrecId == 0)
                {
                    cbi.CommissionRecord = new CommissionRecord();
                    cbi.CommissionRecord.supname = comBoxVM.comrec.supname;
                    cbi.CommissionRecord.comrecId = (int)cbi.comrecId;
                    cbi.CommissionRecord.comrecorddate = cbi.rptdate;
                    cbi.CommissionRecord.status = comBoxVM.comrec.status;
                }
                else
                {
                    cbi.CommissionRecord = db.CommissionRecords.Find(comBoxVM.comrecId);
                }
                db.ComBoxInfoes.Add(cbi);
                await db.SaveChangesAsync();
                logger.Info("Posting of BoxInfoVM Successful. " + comBoxVM.ToString());
                PreBoxInfo pbi = await db.PreBoxInfoes.Where(x => x.lmpdnum == comBoxVM.lmpdnum).FirstOrDefaultAsync();
                if (pbi != null || pbi != default(PreBoxInfo))
                {
                    pbi.status = "Reserved for Reporting";
                    await db.SaveChangesAsync();
                }
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
                ComBoxInfo comBox = await db.ComBoxInfoes.FindAsync(id);
                if (comBox != null)
                {
                    db.ComBoxInfoes.Remove(comBox);
                    await db.SaveChangesAsync();
                    logger.Info("Deletion of BoxInfoVM id(" + id + ") Successful.");
                    PreBoxInfo pbi = await db.PreBoxInfoes.Where(x => x.lmpdnum == comBox.lmpdnum).FirstOrDefaultAsync();
                    if (pbi != null || pbi != default(PreBoxInfo))
                    {
                        pbi.status = "Pre-Commission";
                        await db.SaveChangesAsync();
                    }
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

        public bool ComBoxInfoExists(int id)
        {
            return db.ComBoxInfoes.Count(e => e.comboxId == id) > 0;
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
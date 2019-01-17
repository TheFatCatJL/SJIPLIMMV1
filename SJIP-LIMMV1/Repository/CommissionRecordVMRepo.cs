using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using SJIP_LIMMV1.Models;
using System.Threading.Tasks;
using System.Security.Principal;
using SJIP_LIMMV1.Models.Interfaces;
using AutoMapper;

namespace SJIP_LIMMV1.Repository
{
    public class CommissionRecordVMRepo : IRepository<IViewModel>
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BoxModelEntities db = new BoxModelEntities();

        #region Get Operations
        // Get using ID
        public async Task<IViewModel> GetByID(int id)
        {
            try
            {
                var requestVM = await db.CommissionRecords.FirstOrDefaultAsync(x => x.comrecId == id);
                if (requestVM == null)
                {
                    logger.Info("Commission Record of ID : " + id + " not found.");
                    return null;
                }
                logger.Info("Commission Record of ID : " + id + " found.");
                CommissionRecordVM crvm = Mapper.Map<CommissionRecord, CommissionRecordVM>(requestVM);
                return crvm;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return null;
            }
        }

        // Get using User Obj
        public async Task<IViewModel> GetByUser(IIdentity user)
        {
            try
            {
                var namestr = user.Name;
                var requestVM = await db.CommissionRecords.FirstOrDefaultAsync(x => x.supname == namestr);
                if (requestVM == null)
                {
                    logger.Info("Commission Record of Username : " + namestr + " not found.");
                    return null;
                }
                logger.Info("Commission Record of Username : " + namestr + " found.");
                CommissionRecordVM crvm = Mapper.Map<CommissionRecord, CommissionRecordVM>(requestVM);
                return crvm;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return null;
            }
        }

        // Get using LMPD
        public async Task<IViewModel> GetByLMPD(string lmpd)
        {
            try
            {
                var requestCBI = await db.ComBoxInfoes.Where(x => x.lmpdnum == lmpd).FirstOrDefaultAsync();
                if(requestCBI !=null)
                {
                    var requestVM = await db.CommissionRecords.Where(x => x.comrecId == requestCBI.comrecId).FirstOrDefaultAsync();
                    if (requestVM == null)
                    {
                        logger.Info("Commission Record of LMPD : " + lmpd + " not found.");
                        return null;
                    }
                    logger.Info("Commission Record of LMPD : " + lmpd + " found.");
                    CommissionRecordVM crvm = Mapper.Map<CommissionRecord, CommissionRecordVM>(requestVM);
                    return crvm;
                }
                logger.Info("Commission Record of LMPD : " + lmpd + " not found.");
                return null;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return null;
            }
        }

        // GETs
        public async Task<IEnumerable<IViewModel>> GetAll()
        {
            try
            {
                var requestVMList = await db.CommissionRecords.ToListAsync();
                if (requestVMList == null)
                {
                    logger.Info("Commission Records is empty. Either there is no data or there is unknown error");
                    return null;
                }
                logger.Info("Commission Records found and returned."); 
                List<CommissionRecordVM> crvms = Mapper.Map<List<CommissionRecord>, List<CommissionRecordVM>>(requestVMList);
                return crvms;
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
        public async Task Update(IViewModel record)
        {
            // Retrieve entity base on the LMPD number (which is technically unique and should not double entry)
            try
            {
                CommissionRecordVM recordvm = (CommissionRecordVM)record;
                CommissionRecord cr = Mapper.Map<CommissionRecordVM, CommissionRecord>(recordvm);
                CommissionRecord entity = await db.CommissionRecords.FirstOrDefaultAsync(x => x.comrecId == record.Id);
                if (entity == null)
                {
                    logger.Info("There is no record in database of the Commission Record ID provided (" + record.Id + ")");
                }
                db.Entry(entity).CurrentValues.SetValues(cr);
                await db.SaveChangesAsync();
                logger.Info("Records Updated for Commission Record ID (" + record.Id + ")");
            }
            catch (DbUpdateConcurrencyException exception)
            {
                RefreshDBContextForConcurrencyClientWin();
                if (!CommissionRecordExists(record.Id))
                    logger.Error("An attempted to save Commission Record (" + record.Id + ") failed due to concurrency. Error Trace :" + exception);
                else
                    logger.Error("An attempted to save Commission Record (" + record.Id + ") failed as VM no longer exists. Error Trace :" + exception);
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
            }
        }

        //Submit
        public async Task Create(IViewModel record)
        {
            try
            {
                CommissionRecordVM recordvm = (CommissionRecordVM)record;
                IList<ComBoxInfo> comBoxInfos = Mapper.Map<IList<ComBoxInfoViewModel>, IList<ComBoxInfo>>(recordvm.vms);
                CommissionRecord crDatabase = await db.CommissionRecords.FindAsync(recordvm.Id);
                crDatabase.ComBoxInfoes = comBoxInfos;
                crDatabase.comrecorddate = recordvm.comrecorddate;
                crDatabase.history = recordvm.history;
                crDatabase.status = recordvm.status;
                crDatabase.supname = recordvm.supname;
                foreach(var cbi in crDatabase.ComBoxInfoes)
                {
                    cbi.CommissionRecord = null;
                    cbi.Address = null;
                }                
                db.CommissionRecords.Add(crDatabase);
                await db.SaveChangesAsync();
                logger.Info("Posting of Commission Record Successful. " + record.ToString());
            }
            catch (DbUpdateConcurrencyException e)
            {
                RefreshDBContextForConcurrencyClientWin();
                logger.Error("An attempted to save Commission Record failed due to concurrency. Error Trace :" + e);
            }
            catch (Exception e)
            {
                logger.Error("Error when trying to create a Commission Record entry in database. Error Trace : " + e);
            }
        }

        #endregion
        
        #region Delete Operations
        //Delete
        public async Task Delete(int id)
        {
            try
            {
                CommissionRecord record = await db.CommissionRecords.FindAsync(id);
                if (record != null)
                {
                    db.CommissionRecords.Remove(record);
                    await db.SaveChangesAsync();
                    List<string> pboxeslmpd = record.ComBoxInfoes.Where(x => x.lmpdnum != null).Select(x => x.lmpdnum).ToList();
                    if (pboxeslmpd.Any())
                    {
                        foreach (string pboxlmpd in pboxeslmpd)
                        {
                            PreBoxInfo pbi = await db.PreBoxInfoes.Where(x => x.lmpdnum == pboxlmpd).FirstOrDefaultAsync();
                            if (pbi != default(PreBoxInfo))
                            {
                                pbi.status = "Pre-Commission";                                
                            }
                        }
                        await db.SaveChangesAsync();
                    }
                    logger.Info("Deletion of Commission Record id(" + id + ") Successful.");
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                RefreshDBContextForConcurrencyStoreWin();
                logger.Error("Attempt to delete Commission Record id(" + id + ") failed due to concurrency. Error Trace :" + e);
            }
            catch (Exception e)
            {
                logger.Error("Error when trying to delete Commission Record id(" + id + ") from database. Error Trace : " + e);
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

        public bool CommissionRecordExists(int id)
        {
            return db.CommissionRecords.Count(e => e.comrecId == id) > 0;
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
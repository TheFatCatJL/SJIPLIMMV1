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
    public class ContactFormVMRepo : IRepository<IViewModel>
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BoxModelEntities db = new BoxModelEntities();

        #region Get Operations
        // Get using ID
        public async Task<IViewModel> GetByID(int id)
        {
            try
            {
                ContactForm cf = await db.ContactForms.FirstOrDefaultAsync(x => x.Id == id);
                if (cf == null)
                {
                    logger.Info("Contact Form of ID : " + id + " not found.");
                    return null;
                }
                logger.Info("Contact Form of ID : " + id + " found.");
                ContactFormViewModel cfvm = Mapper.Map<ContactForm, ContactFormViewModel>(cf);
                return cfvm;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return null;
            }
        }

        // Get using LMPD
        public async Task<IViewModel> GetByString(string email)
        {
            try
            {
                ContactForm cf = await db.ContactForms.FirstOrDefaultAsync(x => x.Email== email);
                if (cf == null)
                {
                    logger.Info("ContactFormVM bearing Email : " + email + " not found.");
                    return null;
                }
                logger.Info("ContactFormVM bearing Email : " + email + " found.");
                ContactFormViewModel cfvm = Mapper.Map<ContactForm, ContactFormViewModel>(cf);
                return cfvm;
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
                IList<ContactForm> cfl = await db.ContactForms.ToListAsync();
                if (!cfl.Any())
                {
                    logger.Info("ContactFormVMList is empty. Either there is no data or there is unknown error");
                    return null;
                }
                logger.Info("ContactFormVMList found and returned.");
                IList<ContactFormViewModel> cfvml = Mapper.Map<IList<ContactForm>, IList<ContactFormViewModel>>(cfl);
                return cfvml;
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
            try
            {
                ContactFormViewModel cfvm = (ContactFormViewModel)vm;
                ContactForm cf = Mapper.Map<ContactFormViewModel, ContactForm>(cfvm);
                ContactForm entity = await db.ContactForms.FirstOrDefaultAsync(x => x.Id == cfvm.Id);
                db.Entry(entity).CurrentValues.SetValues(cf);
                await db.SaveChangesAsync();
                logger.Info("ContactForm of ID (" + vm.Id + ") updated.");
            }
            catch (DbUpdateConcurrencyException exception)
            {
                RefreshDBContextForConcurrencyClientWin();
                if (!ContactFormExists(vm.Id))
                    logger.Error("An attempted to save ContactFormVM of ID(" + vm.Id + ") failed due to concurrency. Error Trace :" + exception);
                else
                    logger.Error("An attempted to save ContactFormVM of ID(" + vm.Id + ") failed as VM no longer exists. Error Trace :" + exception);
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
                ContactFormViewModel cfvm = (ContactFormViewModel)vm;
                ContactForm cf = Mapper.Map<ContactFormViewModel, ContactForm>(cfvm);
                db.ContactForms.Add(cf);
                await db.SaveChangesAsync();
                logger.Info("Posting of ContactFormVM Successful. " + cfvm.ToString());
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
                ContactForm cf = await db.ContactForms.FindAsync(id);
                if (cf != null)
                {
                    db.ContactForms.Remove(cf);
                    await db.SaveChangesAsync();
                    logger.Info("Deletion of ContactForm id(" + id + ") Successful.");
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                RefreshDBContextForConcurrencyStoreWin();
                logger.Error("Attempt to delete ContactForm id(" + id + ") failed due to concurrency. Error Trace :" + e);
            }
            catch (Exception e)
            {
                logger.Error("Error when trying to delete ContactForm id(" + id + ") from database. Error Trace : " + e);
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

        public bool ContactFormExists(int id)
        {
            return db.ContactForms.Count(e => e.Id == id) > 0;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;

namespace SJIP_LIMMV1.Manager
{
    public class UserAuthManager : IDisposable
    {
        //IDisposable implementation
        internal bool disposed = false;

        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal HttpContext context;
        // Construct helper with context
        public UserAuthManager(HttpContext context)
        {
            if (context != null)
            {
                this.context = context;
            }
            else
            {
                this.context = HttpContext.Current == null ? null : HttpContext.Current;
            }
        }

        // Helper to manage retrieval of requester IP Address
        private string returnIP()
        {
            var ip = context == null ? null : context.GetOwinContext().Request.RemoteIpAddress;
            var returnString = ip == null ? "Unknown User" : "User IP: " + context.GetOwinContext().Request.RemoteIpAddress;
            if (string.IsNullOrEmpty(ip))
            {
                throw new Exception("User IP is empty or hidden for some reason");
            }
            return returnString;
        }

        // Helper to log annoymous requester Start of Activity
        public bool userAuthenticate(string activity, IPrincipal User)
        {
            // Uses Identity to do a quick auth
            if (User.Identity.IsAuthenticated)
            {
                logger.Info(User.Identity.Name + " started activity (" + activity + ").");
                return true;
            }

            string ip = "";
            // gets user IP for the annoymous, and if fail, logs this failure as an exception said user activity
            try
            {
                ip = returnIP();
                logger.Info(ip + " started activity (" + activity + ").");
                return true;
            }
            catch (Exception e)
            {
                logger.Error("Unknown user started activity(" + activity + "). Error Trace : " + e);
                return false;
            }
        }


        //IDisposable implementation
        public void Dispose()
        {
            Dispose(true);
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
    }
}
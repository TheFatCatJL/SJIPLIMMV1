using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SJIP_LIMMV1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SJIP_LIMMV1.Manager
{
    public class DashboardManager :IDisposable
    {
        #region IDentity operations

        public Boolean isAdminUser(IIdentity identityObj)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));
            var searchForRoles = userManager.GetRoles(identityObj.GetUserId());
            if(searchForRoles[0].ToString() == "Admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string generateUserRole(HttpContextBase context)
        {
            string roleName;
            using (var dbcontext = new ApplicationDbContext())
            {
                string currentid = context.User.Identity.GetUserId();
                string RoleID = dbcontext.Users.Where(user => user.Id == currentid).FirstOrDefault().Roles.FirstOrDefault().RoleId;
                roleName = dbcontext.Roles.Where(r => r.Id == RoleID).FirstOrDefault().Name;
            }
            return roleName;
        }

        public string generateUserEmail(HttpContextBase context)
        {
            string userEmail;
            using (var dbcontext = new ApplicationDbContext())
            {
                string currentid = context.User.Identity.GetUserId();
                userEmail = dbcontext.Users.Where(user => user.Id == currentid).FirstOrDefault().Email;
            }
            return userEmail;
        }


        public string generateUserName(HttpContextBase context)
        {
            return context == null ? null : context.User.Identity.GetUserName();
        }


        public string generateUserID(HttpContextBase context)
        {
            return context == null ? null : context.User.Identity.GetUserId();
        }

        #endregion

        #region Utilities

        public string generateGreeting()
        {
            Random random = new Random();
            int rand = random.Next(1,10);
            switch (rand)
            {
                case 1:
                    return "Going Up..";
                case 2:
                    return "Going Down..";
                case 3:
                    return "Lift doors closing..";
                case 4:
                    return "Have a nice day!";
                case 5:
                    return "Sorry, restricted floor.";
                case 6:
                    return "Pool & Gym deck";
                case 7:
                    return "Basement Carpark";
                case 8:
                    return "Guest Lobby";
                default:
                    return "Please choose access floor";
            }
        }

        //refactor later
        public ViewDataDictionary errorViewBagPrep(RequestContext context, string errorCase)
        {
            UrlHelper helper = new System.Web.Mvc.UrlHelper(context);
            ViewDataDictionary myViewBag = new ViewDataDictionary();
            string url;
            switch (errorCase)
            {
                case "Unauthorise":
                    url = helper.Action("Login", "Account");
                    myViewBag.Add("errorMessage", "Not Logged In");
                    myViewBag.Add("routeString", url);
                    myViewBag.Add("txtString", "Login Now");
                    break;
                case "DeletedEditEntry":
                    url = helper.Action("SubmitBox", "BoxInfo");
                    myViewBag.Add("errorMessage", "The entry you are trying to modify no longer exists.");
                    myViewBag.Add("routeString", url);
                    myViewBag.Add("txtString", "Return to ");
                    break;
                default:
                    myViewBag = null;
                    break;
            }
            return myViewBag;
        }

        #endregion

        #region  IDisposable implementation

        internal bool disposed = false;
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

        #endregion
    }
}
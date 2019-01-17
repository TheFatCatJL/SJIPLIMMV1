using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SJIP_LIMMV1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SJIP_LIMMV1.Manager
{
    public class DashboardManager :IDisposable
    {
        #region IDentity operations

        public async Task<Boolean> isAdminUser(IIdentity identityObj)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));
            var searchForRoles = await userManager.GetRolesAsync(identityObj.GetUserId());
            if (searchForRoles[0].ToString() == "Admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> generateUserRole(HttpContextBase context)
        {
            //wrap around using Task - JS style
            return await Task.Run(() => {
                var dbcontext = new ApplicationDbContext();
                string currentid = context.User.Identity.GetUserId();
                var identityRoleObj = dbcontext.Users.Where(user => user.Id == currentid).FirstOrDefault().Roles.FirstOrDefault();
                string RoleID = identityRoleObj == null ? null : identityRoleObj.RoleId;
                var dbRoleObj = dbcontext.Roles.Where(r => r.Id == RoleID).FirstOrDefault();
                string roleName = dbRoleObj == null ? null : dbRoleObj.Name;
                return roleName;
            });
        }

        public async Task<string> generateUserEmail(HttpContextBase context)
        {
            //wrap around using Task - JS style
            return await Task.Run(() => {
                var dbcontext = new ApplicationDbContext();
                string currentid = context.User.Identity.GetUserId();
                var userObj = dbcontext.Users.Where(user => user.Id == currentid).FirstOrDefault();
                string userEmail = userObj == null ? null : userObj.Email;
                return userEmail;
            });
        }



        public async Task<string> generateUserName(HttpContextBase context)
        {
            return await Task.Run(() => {
                return context == null ? null : context.User.Identity.Name;
            });
        }


        public Task<string> generateUserID(HttpContextBase context)
        {
            return Task.Run(() => {
                return context == null ? null : context.User.Identity.GetUserId();
            });
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
        public async Task<ViewDataDictionary> errorViewBagPrep(RequestContext context, string errorCase)
        {
            return await Task.Run(() => {
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
                        url = helper.Action("PreBoxSubmit", "BoxInfo");
                        myViewBag.Add("errorMessage", "The entry you are trying to modify no longer exists.");
                        myViewBag.Add("routeString", url);
                        myViewBag.Add("txtString", "Return to ");
                        break;
                    default:
                        myViewBag = null;
                        break;
                }
                return myViewBag;
            });
        }

        //refactor later
        public async Task<ViewDataDictionary> pageViewBagPrep(HttpContextBase context, [Optional] string pagename)
        {
            return await Task.Run(() =>
            {
                UrlHelper helper = new System.Web.Mvc.UrlHelper(context.Request.RequestContext);
                ViewDataDictionary myViewBag = new ViewDataDictionary();
                string url;
                switch (pagename)
                {
                    case "Home":
                        url = helper.Action("Index", "Home");
                        var UserName = generateUserName(context).Result;
                        var UserRole = generateUserRole(context).Result;
                        myViewBag.Add("UserRoleName", UserRole);
                        myViewBag.Add("UserGreeting", generateGreeting());
                        myViewBag.Add("routeString", url);
                        myViewBag.Add("Title", "LIMM version 1.0");
                        myViewBag.Add("Message", "LIMM version 1.0");
                        myViewBag.Add("UserLineGreet", String.Format("{0} ({1})", UserName, UserRole));
                        break;
                    case "About":
                        url = helper.Action("About", "Home");
                        myViewBag.Add("Title", "About LIMM version 1.0");
                        myViewBag.Add("Message", "About LIMM version 1.0");
                        myViewBag.Add("routeString", url);
                        break;
                    case "Contact":
                        url = helper.Action("Contact", "Home");
                        myViewBag.Add("routeString", url);
                        myViewBag.Add("Title", "Contact Us");
                        myViewBag.Add("Message", "Please leave us a message and we will get back to you shortly.");
                        break;
                    case "ContactAgain":
                        url = helper.Action("Contact", "Home");
                        myViewBag.Add("routeString", url);
                        myViewBag.Add("Title", "Contact Us");
                        myViewBag.Add("Message", "Thank you for contacting us.\n We will get back to you shortly.");
                        break;
                    case "ContactError":
                        url = helper.Action("Contact", "Home");
                        myViewBag.Add("routeString", url);
                        myViewBag.Add("Title", "Form Submission Error");
                        myViewBag.Add("Message", "Please check the below field(s).");
                        break;
                    default:
                        myViewBag.Add("UserRoleName", "Guest");
                        url = context.Request.Url.PathAndQuery;
                        myViewBag.Add("Title", "You are redirected from " + url);
                        break;
                }
                return myViewBag;
            });           
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
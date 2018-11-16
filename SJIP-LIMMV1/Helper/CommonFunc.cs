using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SJIP_LIMMV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace SJIP_LIMMV1.Helper
{
    public class CommonFunc :IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

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
    }
}
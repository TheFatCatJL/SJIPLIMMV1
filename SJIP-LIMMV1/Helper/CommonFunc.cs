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
    }
}
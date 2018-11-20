using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SJIP_LIMMV1.Models
{
    public class IdentityUser<Tkey, TLogin, TRole, TClaim> : IUser<Tkey>
        where TLogin:IdentityUserLogin<Tkey>
        where TRole:IdentityUserRole<Tkey>
        where TClaim:IdentityUserClaim<Tkey>
    {
        public IdentityUser()
        {
            Claims = new List<TClaim>();
            Roles = new List<TRole>();
            Logins = new List<TLogin>();
        }

        public virtual Tkey Id { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual DateTime? LockoutEndDateUtc { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual int AccessFailedCount { get; set; }
        public virtual ICollection<TRole> Roles { get; private set; }
        public virtual ICollection<TClaim> Claims { get; private set; }
        public virtual ICollection<TLogin> Logins { get; private set; }
        public virtual string UserName { get; set; }
    }
}
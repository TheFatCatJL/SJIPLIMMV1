using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SJIP_LIMMV1.Models;

namespace SJIP_LIMMV1.Repository
{
    public class ContactFormVMContext : DbContext
    {
        public ContactFormVMContext() : base("ModelsDB")
        {
        }

        public DbSet<ContactFormViewModel> ContactFormViewModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactFormViewModel>().HasKey(x => x.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SJIP_LIMMV1.Models;

namespace SJIP_LIMMV1.Repository
{
    public class BoxInfoContext : DbContext
    {
        public BoxInfoContext() : base("ModelsDB")
        {
            
        }

        public DbSet<BoxInfo> BoxInfos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoxInfo>().HasKey(x => x.id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
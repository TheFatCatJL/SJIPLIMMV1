using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SJIP_LIMMV1.Models;

namespace SJIP_LIMMV1.Repository
{
    //public class CommissionRecordContext : DbContext
    //{
    //    public CommissionRecordContext() : base("ModelsDB")
    //    {
    //    }

    //    public DbSet<CommissionRecordVM> CommissionRecords { get; set; }
    //    public DbSet<ComBoxInfoViewModel> ComBoxInfoViewModels { get; set; }

    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<CommissionRecordVM>().HasKey(x => x.Id);
    //        modelBuilder.Entity<ComBoxInfoViewModel>().HasKey(x => x.Id);
    //        base.OnModelCreating(modelBuilder);
    //    }
    //}
}
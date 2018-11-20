using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SJIP_LIMMV1.Models
{
    public class BoxInfoContext:DbContext
    {
        public DbSet<BoxInfoViewModel> BoxInfos { get; set; }.

    }
}
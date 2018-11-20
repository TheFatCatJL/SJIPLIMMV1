using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Data.Entity;

namespace SJIP_LIMMV1.Models
{


    public class BoxInfoViewModel
    {
        public string LMPDNum { get; set; }
        public string SimNum { get; set; }
        public string Telco { get; set; }
        public string JSONID { get; set; }

        public IEnumerable<string> telco = new List<string> { "Singtel", "M1    " };
        public IEnumerable<string> JSONIDNUM = new List<string> { "1", "2    " };

    }
}
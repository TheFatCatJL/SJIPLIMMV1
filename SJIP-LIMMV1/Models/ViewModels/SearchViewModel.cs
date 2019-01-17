using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using SJIP_LIMMV1.Models.Interfaces;

namespace SJIP_LIMMV1.Models
{
    public class SearchViewModel : IViewModel
    {
        public string searchstring { get; set; }

        [Display(Name = "Placeholder")]
        public string Placeholder { get; set; }
        [Display(Name = "Block")]
        public string Block { get; set; }
        [Display(Name = "LMPD")]
        public string LMPD { get; set; }
        [Display(Name = "Simcard")]
        public string SIMCard { get; set; }

        public BoxInfoRecord records { get; set; }
        public string radiochoice { get; set; }
        public int Id { get; set; }

        public SearchViewModel()
        {
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (this.searchstring == null)
            {
                results.Add(new ValidationResult("Search String msut not be empty for submission", new string[] { "searchstring" }));
            }
            return results;
        }
    }
}
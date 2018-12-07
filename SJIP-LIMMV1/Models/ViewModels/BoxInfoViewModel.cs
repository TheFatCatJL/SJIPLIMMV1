using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Data.Entity;
using System;
using System.Text;
using SJIP_LIMMV1.Models.Interfaces;
using System.Collections;
using System.Web.Mvc;

namespace SJIP_LIMMV1.Models
{
    public class BoxInfoViewModel : IViewModel, IValidatableObject
    {
        // IViewModel Implementation
        internal ViewDataDictionary viewdata;
        public ViewDataDictionary viewDataExtra { get => viewdata; set => viewdata = value; }

        [Key]
        [Display(Name = "Id Key")]
        public int id { get; set; }

        [Required(ErrorMessage ="LMPD number must be present for submission")]
        [Display(Name = "LMPD #")]
        public string lmpdnum { get; set; }
        [Display(Name = "Simcard #")]
        public string simnum { get; set; }
        [Display(Name = "Telco (Simcard)")]
        public string telco { get; set; }
        [Display(Name = "JSON File number")]
        public string jsonid { get; set; }
        [Display(Name = "Checker Name")]
        public string checkername { get; set; }
        [Display(Name = "Technician Name")]
        public string techname { get; set; }
        [Display(Name = "Date Checked")]
        public DateTime? checkdate { get; set; }
        [Display(Name = "Date Installed")]
        public DateTime? installdate { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "History")]
        public string history { get; set; }

        [Display(Name = "Status list")]
        public IEnumerable<string> statuslist { get => new List<string> { "Pending check", "Pre-Commission", "Commissioned", "In Repair", "KIV", "Decommissioned" }; }
        [Display(Name = "Telco list (Simcard)")]
        public IEnumerable<string> telcolist { get => new List<string> { "Singtel", "MobileOne", "Tata", "Starhub", "Circles" }; }
        [Display(Name = "JSON Files")]
        public IEnumerable<string> jsonlist { get => new List<string> { "13242", "23221", "333345", "P235663", "235KKL" }; }

        public override string ToString()
        {
            StringBuilder buildText = new StringBuilder();
            buildText.AppendLine("ID # " + id);
            buildText.AppendLine("LMPD # " + lmpdnum);
            buildText.AppendLine("Simcard # " + simnum);
            buildText.AppendLine("Telco : " + telco);
            buildText.AppendLine("JSONID :" + jsonid);
            buildText.AppendLine("Checker Name :" + checkername);
            buildText.AppendLine("Tech Name :" + techname);
            buildText.AppendLine("Check Date :" + checkdate);
            buildText.AppendLine("Install Date :" + installdate);
            return buildText.ToString();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Validate the DateEncaissment
            if (this.lmpdnum == null)
            {
                results.Add(new ValidationResult("LMPD number must be present for submission", new string[] { "LMPD" }));
            }
            return results;
        }
    }
}
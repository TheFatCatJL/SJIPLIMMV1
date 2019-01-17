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
    public class ComBoxInfoViewModel : IViewModel
    {
        [Key]
        [Display(Name = "Id Key")]
        public int Id { get; set; }

        [Display(Name = "Commission Record")]
        public virtual CommissionRecordVM comrec { get; set; }

        [Display(Name = "Commission Record ID")]
        public virtual int comrecId { get; set; }

        [Required(ErrorMessage ="LMPD number must be present for submission")]
        [Display(Name = "LMPD #")]
        public string lmpdnum { get; set; }

        [Required(ErrorMessage = "Please enter postal code")]
        [Display(Name = "Postal Code")]
        public string rptpostalcode { get; set; }
        [Required(ErrorMessage = "Please indicate the lift")]
        [Display(Name = "Lift")]
        public string rptlift { get; set; }

        [Display(Name = "Date Commissioned")]
        public DateTime? rptcomdate { get; set; }
        [Display(Name = "Comments")]
        public string rptcomment { get; set; }

        [Display(Name = "Team Name")]
        public string teamname { get; set; }
        [Display(Name = "Technician Name")]
        public string techname { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "History")]
        public string history { get; set; }
        [Display(Name = "Matched Flag")]
        public bool ismatched { get; set; }

        [Display(Name = "Lift list")]
        public IList<string> liftlist { get => new List<string> { "A", "B", "C" }; }

        public override string ToString()
        {
            StringBuilder buildText = new StringBuilder();
            buildText.AppendLine("ID # " + Id);
            buildText.AppendLine("LMPD # : " + lmpdnum);
            buildText.AppendLine("Team Name : " + teamname);
            buildText.AppendLine("Tech Name :" + techname);
            buildText.AppendLine("Rpt Postal Code :" + rptpostalcode);
            buildText.AppendLine("Rpt Lift :" + rptlift);
            buildText.AppendLine("Commission Date :" + rptcomdate);
            buildText.AppendLine("Comments :" + rptcomment);
            buildText.AppendLine("Status :" + status);
            buildText.AppendLine("History :" + history);
            return buildText.ToString();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (this.lmpdnum == null)
                results.Add(new ValidationResult("LMPD must be valid for submission", new string[] { "LMPD" }));
            if (this.rptpostalcode == null)
                results.Add(new ValidationResult("Postal Code must be valid for submission", new string[] { "PostalCode" }));
            if (this.rptlift == null)
                results.Add(new ValidationResult("Lift must be valid for submission", new string[] { "Lift" }));
            return results;
        }
    }
}
using SJIP_LIMMV1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SJIP_LIMMV1.Models
{
    public class CommissionRecordVM : IViewModel
    {
        [Key]
        [Display(Name = "Commission Record ID")]
        public int Id { get; set; }

        [Display(Name = "Supervisor Name")]
        public string supname { get; set; }

        [Display(Name = "Commission Report Date")]
        [Required(ErrorMessage ="Please enter valid date")]
        public DateTime comrecorddate { get; set; }

        [Display(Name = "Comments")]
        public string comment { get; set; }
        [Display(Name = "History")]
        public string history { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }


        [Display(Name = "List of Commissioned LMPD")]
        public virtual IList<ComBoxInfoViewModel> vms { get; set; }

        public override string ToString()
        {
            StringBuilder buildText = new StringBuilder();
            buildText.AppendLine("ID # " + Id);
            buildText.AppendLine("Sup Name " + supname);
            buildText.AppendLine("Commission Report Date :" + comrecorddate);
            buildText.AppendLine("Comments :" + comment);
            buildText.AppendLine("Status :" + status);
            return buildText.ToString();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (this.comrecorddate == null)
            {
                results.Add(new ValidationResult("Reporting Date must be present for submission", new string[] { "ComDate" }));
            }
            return results;
        }
    }
}
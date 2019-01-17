using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJIP_LIMMV1.Models.Interfaces;

namespace SJIP_LIMMV1.Models
{
    public class ContactFormViewModel : IViewModel
    {
        [Key]
        [Display(Name = "Id Key")]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Your email is required.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Your name is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Message cannot be empty.")]
        [Display(Name = "Message")]
        public string Message { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if(this.Email == null)
                results.Add(new ValidationResult("Email must be valid/present for submission", new string[] { "Email" }));
            if (this.Name == null)
                results.Add(new ValidationResult("Name must be present for submission", new string[] { "Name" }));
            if (this.Message == null)
                results.Add(new ValidationResult("Message must be present for submission", new string[] { "Message" }));
            return results;
        }
    }
}
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
        ViewDataDictionary extradetails;

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

        public ViewDataDictionary viewDataExtra { get => extradetails; set => extradetails = value; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SJIP_LIMMV1.Models.Interfaces
{
    public interface IViewModel : IValidatableObject
    {
        [Key]
        [Display(Name = "Id Key")]
        int Id { get; set; }
    }
}

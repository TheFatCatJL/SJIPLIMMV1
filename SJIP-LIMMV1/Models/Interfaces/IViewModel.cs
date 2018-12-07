using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SJIP_LIMMV1.Models.Interfaces
{
    public interface IViewModel
    {
        // Used as a container for temp ErrorMsg and Viewbag customisations implementation
        ViewDataDictionary viewDataExtra { get; set; }
    }
}

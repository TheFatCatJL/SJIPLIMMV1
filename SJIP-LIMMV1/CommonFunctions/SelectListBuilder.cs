using SJIP_LIMMV1.Manager;
using SJIP_LIMMV1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SJIP_LIMMV1.Common
{
    public static class SelectListBuilder
    {
        static List<SelectListItem> myList;
        static HttpContextBase context;
        // build select list for dropdownlist which requires select list item to be managed (eg. LMPD)
        static SelectListBuilder ()
        {
            myList = new List<SelectListItem>();
        }

        public static void Setup(HttpContextBase contextBase)
        {
            context = contextBase;
            IEnumerable<PreBoxInfoViewModel> vms = Task.Run(async () => await new ViewModelManager<PreBoxInfoViewModel>(contextBase).GetAll()).Result;
            List<PreBoxInfoViewModel> listpbvm = vms.ToList().Where(x => x.status == "Pre-Commission").ToList();
            myList = listpbvm.Select(s => new SelectListItem { Value = s.lmpdnum, Text = s.lmpdnum }).ToList();
        }

        public static List<SelectListItem> Pop(string lmpd)
        {
            SelectListItem finditem = myList.Find(x => x.Value == lmpd);
            if (finditem !=null)
            {
                myList.Remove(finditem);
            }
            return myList;
        }

        public static List<SelectListItem> Push(string lmpd)
        {
            SelectListItem finditem = myList.Find(x => x.Value == lmpd);
            if (finditem == null)
            {                
                myList.Add(new SelectListItem() { Text = lmpd, Value = lmpd });
                PreBoxInfoViewModel pbvm = Task.Run(async () => await new ViewModelManager<PreBoxInfoViewModel>(context).GetOneBySpecial(lmpd)).Result;
                if (pbvm != null || pbvm != default(PreBoxInfoViewModel))
                {
                    pbvm.status = "Pre-Commission";
                    Task.Run(async () => await new ViewModelManager<PreBoxInfoViewModel>(context).Post(pbvm));
                }
            }
            return myList;
        }

        public static List<SelectListItem> GetListItems()
        {
            return myList;
        }

    }
   
}
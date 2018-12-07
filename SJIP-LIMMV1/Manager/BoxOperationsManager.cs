using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SJIP_LIMMV1.Manager
{
    public class BoxOperationsManager
    {

        // Declare logger - Note standard declaration
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal BoxInfoViewModel vm;
        public static IDictionary<string, object> PackingDetails { get; set; }

        // Makes ViewModel
        private static IDictionary<string, Func<BoxInfoViewModel, BoxInfoViewModel>> PackBox = new Dictionary<string, Func<BoxInfoViewModel, BoxInfoViewModel>>
        {
            {
                "BoxInfoInitial", (box) => 
                {
                    box.status = "Pre-Commission";
                    return box;
                }
            },
            {
                "BoxSubmmission", (box) => 
                {
                    box.checkername = (string) PackingDetails["checkername"];
                    box.checkdate = DateTime.Now;
                    box.history = string.Format("Pre-Commission done by {0} on {1}.", box.checkername, box.checkdate);
                    return box;
                }
            }
        };

        public BoxOperationsManager(string orderType, [Optional] IDictionary<string, object> packingdetails, [Optional] BoxInfoViewModel box)
        {
            try
            {
                PackingDetails = packingdetails;
                if(box == null)
                {
                    box = new BoxInfoViewModel();
                }
                this.vm = PackBox[orderType].Invoke(box);
            }
            catch (Exception e)
            {
                this.vm = null;
                logger.Error("Error when trying to pack box. Error Trace :" + e);
            }
        }

        public BoxInfoViewModel getPackage()
        {
            return vm;
        }
    }
}
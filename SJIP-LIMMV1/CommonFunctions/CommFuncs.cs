using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SJIP_LIMMV1.Common
{
    public static class CommFuncs
    {
        // render HTML view into string for JSON passing between ajax call and controllers
        // necessary If one needs to pass more data from controller with partial views
        public static string RenderViewToString(this Controller controller, string viewName, object model)
        {
            using (var writer = new StringWriter())
            {
                // gets the controller context and creates the view
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);

                // sets the View Model
                controller.ViewData.Model = model;

                // Base on the View of the ViewResult, model, ViewData+Tempdata (internal viewbag values) and the controller context, prepare a view context using the writer
                var viewCxt = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, writer);

                // renders the view context using the writer. Note the writer for above and here is meant for a placeholder for "value" while the whole wooha is put together
                viewCxt.View.Render(viewCxt, writer);

                // finally, sents back the whole view as a string
                return writer.ToString();
            }
        }
    }

   
}
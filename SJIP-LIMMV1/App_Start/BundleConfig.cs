using System.Web;
using System.Web.Optimization;

namespace SJIP_LIMMV1
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"));
                        
            bundles.Add(new ScriptBundle("~/bundles/jsprologue").Include(
                        "~/Scripts/js/breakpoints.min.js",
                        "~/Scripts/js/browser.min.js",
                        "~/Scripts/js/jquery.min.js",
                        "~/Scripts/js/jquery.scrollex.min.js",
                        "~/Scripts/js/jquery.scrolly.min.js",
                        "~/Scripts/js/main.js",
                        "~/Scripts/js/util.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/mvcgrid").Include(
                        "~/Scripts/MvcGrid/mvc-grid.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/cssprologue").Include(
                        "~/Content/css/font-awesome.min.css",
                        "~/Content/css/main.css"));

            bundles.Add(new StyleBundle("~/Content/cssgrid").Include(
                        "~/Content/MvcGrid/mvc-grid.css"));
        }
    }
}

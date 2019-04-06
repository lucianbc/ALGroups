using System.Web;
using System.Web.Optimization;

namespace ALGroups
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
                        "~/Scripts/moment.js",
                        "~/Scripts/user/validations.js"));
            bundles.Add(new ScriptBundle("~/bundles/calendar").Include(
                        "~/Scripts/moment.js",
                        "~/Scripts/fullcalendar.js",
                        "~/Scripts/qTip/jquery.qtip.js",
                        "~/Scripts/user/mycalendar.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"));
            bundles.Add(new StyleBundle("~/Content/groupcss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/navbar-fixed-left.css",
                      //"~/Content/fullcalendar.print.css",
                      "~/Content/fullcalendar.css",
                      "~/Scripts/qTip/jquery.qtip.css"
                      ));
        }
    }
}

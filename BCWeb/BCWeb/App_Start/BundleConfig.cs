using System.Web.Optimization;

namespace BCWeb
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/cvl").Include(
                        "~/Scripts/cvl.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/cvlcss").Include("~/Content/cvl.css"));

            bundles.Add(new ScriptBundle("~/bundles/angulerjs").Include("~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/projectindex").Include("~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/ProjectIndexCtrl.js"));

            #region Foundation Bundles
            //If your project requires jQuery, you may remove the zepto bundle

            bundles.Add(new StyleBundle("~/Content/foundation/css").Include(
                       "~/Content/foundation/foundation.css",
                       "~/Content/foundation/foundation.mvc.css",
                       "~/Content/foundation/app.css"));

            bundles.Add(new ScriptBundle("~/bundles/foundation").Include(
                      "~/Scripts/foundation/foundation.js",
                      "~/Scripts/foundation/foundation.*",
                      "~/Scripts/foundation/app.js"));
            #endregion
        }
    }
}
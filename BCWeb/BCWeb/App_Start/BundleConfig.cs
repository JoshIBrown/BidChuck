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

            bundles.Add(new StyleBundle("~/Content/jqueryuicss").Include(
                    "~/Content/themes/base/jquery-ui.css"));

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

            #region Projects
            bundles.Add(new ScriptBundle("~/bundles/projectindex").Include("~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/ProjectIndexCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/createproject").Include("~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/CreateProjectCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/editproject").Include("~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/EditProjectCtrl.js"));


            bundles.Add(new ScriptBundle("~/bundles/projectbpdetail").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/ProjectBPDetailsCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/projectsubvenddetail").Include(
                "~/Scripts/moment.js",
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/ProjectSubVendDetailsCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/createprojectsimple").Include(
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jQuery/CreateProject.js"
                ));
            #endregion

            #region Bid Packages
            bundles.Add(new ScriptBundle("~/bundles/bidpackagecreate").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/CreateBidPackageCtrl.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bidpackageedit").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/EditBidPackageCtrl.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bidpackageindex").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/BidPackageIndexCtrl.js"
                ));
            #endregion

            bundles.Add(new ScriptBundle("~/bundles/sendinvitation").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/SendInvitationCtrl.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/scopepicker").Include("~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/ScopesList.js"));

            bundles.Add(new ScriptBundle("~/bundles/home").Include("~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/HomeCtrl.js"));

            #region Foundation Bundles
            //If your project requires jQuery, you may remove the zepto bundle

            bundles.Add(new StyleBundle("~/Content/foundation/css").Include(
                       "~/Content/foundation/foundation.css",
                       "~/Content/foundation/foundation.mvc.css",
                       "~/Content/foundation/app.css"));

            bundles.Add(new ScriptBundle("~/bundles/foundation").Include(
                      "~/Scripts/BidChuck.js",
                      "~/Scripts/foundation/foundation.js",
                      "~/Scripts/foundation/foundation.*",
                      "~/Scripts/foundation/app.js"));
            #endregion


            #region admin bundles
            bundles.Add(new ScriptBundle("~/bundles/admin/userProfile").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/DataTables/media/js/jquery.dataTables.js",
                "~/Scripts/angular/directives/bcweb.angular.DataTables.js",
                "~/Scripts/angular/controllers/admin/UserProfileCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/CompanyProfile").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/DataTables/media/js/jquery.dataTables.js",
                "~/Scripts/angular/directives/bcweb.angular.DataTables.js",
                "~/Scripts/angular/controllers/admin/CompanyProfileCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/project").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/DataTables/media/js/jquery.dataTables.js",
                "~/Scripts/angular/directives/bcweb.angular.DataTables.js",
                "~/Scripts/angular/controllers/admin/ProjectCtrl.js"));
            #endregion
        }
    }
}
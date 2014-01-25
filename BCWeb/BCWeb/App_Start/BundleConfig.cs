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

            #region BIDs
            bundles.Add(new ScriptBundle("~/bundles/receivedbids").Include(
                "~/Scripts/angular/angular.js",
                 "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/ReceivedBidCtrl.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/svcomposebid").Include(
                "~/Scripts/angular/angular.js",
                 "~/Scripts/angular/bcweb.filters.angular.js",
                 "~/Scripts/angular/controllers/SVComposeBid.js"
                ));
            #endregion

            #region Projects
            bundles.Add(new ScriptBundle("~/bundles/projectindex").Include("~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/ProjectIndexCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/editproject").Include("~/Scripts/angular/angular.js",
                "~/Scripts/moment.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/directives/bcweb.angular.ScopePicker.js",
                "~/Scripts/angular/controllers/EditProjectCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/createproject").Include("~/Scripts/angular/angular.js",
                "~/Scripts/moment.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/directives/bcweb.angular.ScopePicker.js",
                "~/Scripts/angular/controllers/CreateProjectCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/projectbpdetail").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/ProjectBPDetailsCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/projectsubvenddetail").Include(
                "~/Scripts/moment.js",
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/ProjectSubVendDetailsCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/createprojectstepone").Include(
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jQuery/CreateProject.js"
                ));
            #endregion

            #region Bid Packages
            bundles.Add(new ScriptBundle("~/bundles/bidpackagecreate").Include(
                "~/Scripts/moment.js",
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/directives/bcweb.angular.ScopePicker.js",
                "~/Scripts/angular/controllers/CreateBidPackageCtrl.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bidpackageedit").Include(
                "~/Scripts/moment.js",
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/directives/bcweb.angular.ScopePicker.js",
                "~/Scripts/angular/controllers/EditBidPackageCtrl.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bidpackageindex").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/BidPackageIndexCtrl.js"
                ));
            #endregion

            #region Invitation
            bundles.Add(new ScriptBundle("~/bundles/sendinvitation").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/SendInvitationCtrl.js"
                ));
            #endregion

            #region account management
            bundles.Add(new ScriptBundle("~/bundles/companyscopes").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/directives/bcweb.angular.ScopePicker.js",
                "~/Scripts/angular/controllers/CompanyScopesCtrl.js"));
            #endregion

            #region home
            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/angular/controllers/HomeCtrl.js"));
            #endregion

            #region Foundation Bundles
            //If your project requires jQuery, you may remove the zepto bundle

            bundles.Add(new StyleBundle("~/Content/foundation/css").Include(
                       "~/Content/foundation/foundation.css",
                       "~/Content/foundation/foundation.mvc.css",
                       "~/Content/foundation/app.css"));

            bundles.Add(new ScriptBundle("~/bundles/foundation").Include(
                      "~/Scripts/BidChuck.js",
                      "~/Scripts/foundation/foundation.js",
                      "~/Scripts/foundation/foundation.*"));
            #endregion

            #region Datatables bundles
            bundles.Add(new StyleBundle("~/Content/dataTables").Include("~/Content/foundation/dataTables.foundation.css"));
            #endregion

            #region admin bundles
            bundles.Add(new ScriptBundle("~/bundles/admin/userProfile").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/DataTables/media/js/jquery.dataTables.js",
                "~/Scripts/DataTables/media/js/dataTables.foundation.js",
                "~/Scripts/angular/directives/bcweb.angular.DataTables.js",
                "~/Scripts/angular/controllers/admin/UserProfileCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/CompanyProfile").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/DataTables/media/js/jquery.dataTables.js",
                "~/Scripts/DataTables/media/js/dataTables.foundation.js",
                "~/Scripts/angular/directives/bcweb.angular.DataTables.js",
                "~/Scripts/angular/controllers/admin/CompanyProfileCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/project").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/bcweb.filters.angular.js",
                "~/Scripts/DataTables/media/js/jquery.dataTables.js",
                "~/Scripts/DataTables/media/js/dataTables.foundation.js",
                "~/Scripts/angular/directives/bcweb.angular.DataTables.js",
                "~/Scripts/angular/controllers/admin/ProjectCtrl.js"));
            #endregion

            #region Search
            bundles.Add(new ScriptBundle("~/bundles/search").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/controllers/SearchCtrl.js"));

            #endregion

            #region notification
            bundles.Add(new ScriptBundle("~/bundles/allnotifications").Include(
                "~/Scripts/moment.js",
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/controllers/NotificationCtrl.js"
                ));
            #endregion

            #region Profile
            bundles.Add(new ScriptBundle("~/bundles/profile").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/controllers/ProfileCtrl.js"));
            #endregion
        }
    }
}
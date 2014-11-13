namespace Yashmak.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            RegisterScripts(bundles);
            RegisterStyles(bundles);

            BundleTable.EnableOptimizations = false;
        }

        private static void RegisterStyles(BundleCollection bundles)
        {
            bundles.Add(
                new StyleBundle("~/Content/kendo").Include(
                    "~/Content/kendo/kendo.common-bootstrap.min.css", 
                    "~/Content/kendo/kendo.flat.min.css"));

            bundles.Add(new StyleBundle("~/Content/site").Include("~/Content/site.css"));

            bundles.Add(
                new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.css", 
                    "~/Content/bootstrap.flatly.css", 
                    "~/Content/font-awesome.css"));
        }

        private static void RegisterScripts(BundleCollection bundles)
        {
            bundles.Add(
                new ScriptBundle("~/bundles/kendo").Include(
                    "~/Scripts/kendo/kendo.all.min.js", 
                    "~/Scripts/kendo/kendo.aspnetmvc.min.js"));

            bundles.Add(
                new ScriptBundle("~/bundles/jquery").Include(
                    "~/Scripts/jquery-{version}.js", 
                    "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(
                new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

            bundles.Add(
                new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(
                new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.js", 
                    "~/Scripts/respond.js"));
        }
    }
}
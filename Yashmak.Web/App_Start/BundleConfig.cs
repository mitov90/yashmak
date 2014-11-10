namespace Yashmak.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScripts(bundles);

            RegisterStyles(bundles);


            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }

        private static void RegisterStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/site").Include(
                "~/Content/site.css"));

            bundles.Add(
                new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/bootstrap.flatly.css",
                    "~/Content/font-awesome.css"));

            bundles.Add(
                new StyleBundle("~/Content/dropzone").Include(
                    "~/Scripts/dropzone/css/basic.css",
                    "~/Scripts/dropzone/css/dropzone.css"));
        }

        private static void RegisterScripts(BundleCollection bundles)
        {
            bundles.Add(
                new ScriptBundle("~/bundles/jquery").Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include("~/Scripts/dropzone/dropzone.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));
        }
    }
}
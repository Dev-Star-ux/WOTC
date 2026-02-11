using System.Web.Optimization;

namespace StaffZoneMaster
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/jquery-unobtrusive-ajax").Include("~/Scripts/jquery.unobtrusive-ajax.js"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
            bundles.Add(new Bundle("~/bundles/bootstrap-min").Include("~/Scripts/bootstrap.min.js"));
            bundles.Add(new Bundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.bundle.js"));
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrap-select").Include("~/Content/bootstrap-select.css"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-select").Include("~/Scripts/bootstrap-select.js"));
            bundles.Add(new StyleBundle("~/Content/font-awesome").Include("~/Content/font-awesome.css"));
            bundles.Add(new ScriptBundle("~/bundles/moment").Include("~/Scripts/moment.js", "~/Scripts/moment-with-locales.js", "~/Scripts/moment-timezone.js"));
            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include("~/Scripts/bootstrap-datetimepicker.min.js"));
            bundles.Add(new StyleBundle("~/Content/datetimepicker").Include("~/Content/bootstrap-datetimepicker.min.css"));
            bundles.Add(new Bundle("~/bundles/datatables").Include("~/Scripts/datatables.js"));
            bundles.Add(new StyleBundle("~/Content/datatables").Include("~/Content/datatables.css"));
            bundles.Add(new StyleBundle("~/Content/dropzone").Include("~/Scripts/dropzone/basic.css", "~/Scripts/dropzone/dropzone.css"));
            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include("~/Scripts/dropzone/dropzone.js"));
            bundles.Add(new StyleBundle("~/Content/image-uploader").Include("~/Content/image-uploader.css"));
            bundles.Add(new ScriptBundle("~/bundles/image-uploader").Include("~/Scripts/image-uploader/image-uploader.js"));
            bundles.Add(new StyleBundle("~/Content/custom").Include("~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/custom/main").Include("~/Scripts/custom/main.js"));
            bundles.Add(new ScriptBundle("~/bundles/custom/home").Include("~/Scripts/custom/home.js"));
            bundles.Add(new ScriptBundle("~/bundles/custom/application").Include("~/Scripts/custom/application.js"));
            bundles.Add(new ScriptBundle("~/bundles/custom/image-uploader").Include("~/Scripts/custom/image-uploader.js"));
            bundles.Add(new ScriptBundle("~/bundles/custom/dynamsoft-webtwain-config").Include("~/Scripts/custom/Resources/dynamsoft.webtwain.config.js"));
            bundles.Add(new ScriptBundle("~/bundles/custom/dynamsoft-webtwain-initiate").Include("~/Scripts/custom/Resources/dynamsoft.webtwain.initiate.js"));
            bundles.Add(new ScriptBundle("~/bundles/custom/dynamsoft-webtwain-addon").Include("~/Scripts/custom/Resources/addon/dynamsoft.webtwain.addon.pdf.js"));
            bundles.Add(new ScriptBundle("~/bundles/custom/dynamsoft-webtwain-common").Include("~/Scripts/custom/Scripts/common.js"));
            bundles.Add(new ScriptBundle("~/bundles/custom/dynamsoft-webtwain-initpage").Include("~/Scripts/custom/Scripts/online_demo_initpage.js"));
            bundles.Add(new ScriptBundle("~/bundles/custom/dynamsoft-webtwain-operation").Include("~/Scripts/custom/Scripts/online_demo_operation.js"));
            bundles.Add(new StyleBundle("~/Content/dynamsoft-fonts").Include("~/Content/scanner/fonts.css"));
            bundles.Add(new StyleBundle("~/Content/dynamsoft-style").Include("~/Content/scanner/style.css"));
            bundles.Add(new StyleBundle("~/Content/dynamsoft-webtwain").Include("~/Content/scanner/dynamsoft.webtwain.css"));
            bundles.Add(new StyleBundle("~/Content/dynamsoft-webtwain-viewer").Include("~/Content/scanner/dynamsoft.webtwain.viewer.css"));
        }
    }

}

using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;
using log4net.Config;
using StaffZoneMaster.Helpers;

namespace StaffZoneMaster.App_Start
{
    public class MvcApplication : HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            XmlConfigurator.Configure();
        }

        protected void Application_BeginRequest()
        {
            base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            base.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1.0));
            base.Response.Cache.SetNoStore();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = base.Server.GetLastError();
            LoggerHelper.Error(Log, ex);
            base.Response.Clear();
            HttpException httpException = ex as HttpException;
            int errorCode = httpException?.GetHttpCode() ?? 0;
            base.Server.ClearError();
            if (httpException is HttpAntiForgeryException)
            {
                base.Response.Redirect("/Auth/Login");
            }
            else
            {
                base.Response.RedirectToRoute("/Error", new
                {
                    ErrorCode = errorCode,
                    Message = ((errorCode != 404) ? ex.Message : "")
                });
            }
        }
    }

}

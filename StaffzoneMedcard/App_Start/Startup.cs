using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace StaffZoneMaster
{
    public class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Auth/Login"),
                ExpireTimeSpan = TimeSpan.FromHours(24.0)
            });
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }

}


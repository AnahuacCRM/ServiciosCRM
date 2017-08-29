using Baner.Recepcion.Services;
using Baner.Recepcion.Services.Seguridad;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(Startup))]
namespace Baner.Recepcion.Services
{
    public class Startup
    {

        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            var oauthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/o/Server"),
                Provider = new RhinoOauthServerProvider(),
                AuthorizationCodeExpireTimeSpan = TimeSpan.FromHours(24),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(24),
                SystemClock = new SystemClock(),


            };
            app.UseOAuthAuthorizationServer(oauthOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Baner.Recepcion.Services.Seguridad
{
    public class RhinoOauthServerProvider : OAuthAuthorizationServerProvider
    {

        public override  Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var usr = ConfigurationManager.AppSettings["oauthusr"];
            var pwd = ConfigurationManager.AppSettings["oauthpwd"];


            if (context.UserName == usr) // && context.Password == pwd)
            {
                var claimsIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                claimsIdentity.AddClaim(new Claim("user", context.UserName));
                context.Validated(claimsIdentity);

                //return;
                return Task.FromResult(0);
            }
            context.Rejected();
            return Task.FromResult(0);

            //context.Validated();
            //return;
        }

        public override  Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var secretunhashed = ConfigurationManager.AppSettings["rhino"];
            var secrethashed = ConfigurationManager.AppSettings["secret"];

            string clientId;
            string clientSecret;
            if (context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                //bool matches = BCrypt.Net.BCrypt.Verify(secretunhashed, clientSecret);

                //if (matches)
                if (clientId == "Rhino" && clientSecret == secrethashed)
                {
                    context.Validated();
                }
            }
            return Task.FromResult(0);
        }
    }
}
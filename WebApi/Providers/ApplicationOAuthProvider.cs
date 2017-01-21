using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebApi.AuthIdentity;

namespace WebApi.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            var user = await userManager.FindAsync(context.UserName.ToLower(), context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            try
            {
                var oAuthIdentity = await userManager.CreateIdentityAsync(user, context.Options.AuthenticationType);
                var cookiesIdentity = await userManager.CreateIdentityAsync(user,
                    CookieAuthenticationDefaults.AuthenticationType);
                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "client_id", context.ClientId == null ? string.Empty : context.ClientId
                    },
                    {
                        "userName", context.UserName
                    }
                });
                var ticket = new AuthenticationTicket(oAuthIdentity, props);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            catch (Exception ex)
            {
                Trace.TraceError("FUSION Error ::: " + ex.Message + ex.InnerException);
                Trace.TraceError(ex.Message);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
                if (property.Value != null)
                    context.AdditionalResponseParameters.Add(property.Key, property.Value);

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
                context.Validated();

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(ApplicationUser userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {"userName", userName.UserName},
                {"email", userName.Email}
                //{ "userImageUrl", userName.userImageUrl},
                //{ "contact", userName.phoneNo},
                //{ "lastname", userName.LastName},
                //{ "firstname", userName.FirstName },
                //{ "roles", userName.roles[0]},
                //{ "userid", userName.userId}
            };
            return new AuthenticationProperties(data);
        }
    }
}
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactsList.API
{
    public static class ServicePrincipal
    {
        // The authority is the issuer URL of the tenant.
        static string authority = ConfigurationManager.AppSettings["ida:Authority"];

        // The resource is the Client ID of the "data web api" AAD app.
        static string resource = ConfigurationManager.AppSettings["ida:Resource"];

        // The Client ID of the "client" AAD app (i.e. this app).
        static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];

        // The key that was created for the "client" app (i.e. this app).
        static string clientSecret = ConfigurationManager.AppSettings["ida:ClientSecret"];

        public static AuthenticationResult GetS2SAccessTokenForProdMSA()
        {
            return GetS2SAccessToken(authority, resource, clientId, clientSecret);
        }


        ///<summary>
        /// Gets an application token used for service-to-service (S2S) API calls.
        ///</summary>
        static AuthenticationResult GetS2SAccessToken(string authority, string resource, string clientId, string clientSecret)
        {
            // Client credential consists of the "client" AAD web application's Client ID
            // and the key that was generated for the application in the AAD Azure portal extension.
            var clientCredential = new ClientCredential(clientId, clientSecret);

            // The authentication context represents the AAD directory.
            AuthenticationContext context = new AuthenticationContext(authority, false);

            // Fetch an access token from AAD.
            AuthenticationResult authenticationResult = context.AcquireToken(
                resource,
                clientCredential);
            return authenticationResult;
        }
    }
}
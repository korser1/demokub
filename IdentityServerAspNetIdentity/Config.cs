// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServerAspNetIdentity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };


        public static IEnumerable<ApiResource> Apis(AppConfiguration config)
        {
            var apiResource = new ApiResource(config.Audience, "My API");
            apiResource.Scopes.Add(config.Scope);
            return new List<ApiResource>
            {
                apiResource
            };
        }

        public static IEnumerable<ApiScope> Scopes(AppConfiguration config)
        {
            var apiScope = new ApiScope(config.Scope);
            return new List<ApiScope>
            {
                apiScope
            };
        }

        public static IEnumerable<Client> Clients(AppConfiguration config) =>
            new List<Client>
            {
                // machine to machine client
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // scopes that client has access to
                    AllowedScopes = { config.Scope }
                },
                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = config.ClientId,
                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,

                    // where to redirect to after login
                    RedirectUris = config.Redirects,

                    // where to redirect to after logout
                    PostLogoutRedirectUris = config.Callbacks,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        config.Scope
                    },

                    AllowOfflineAccess = true
                },
                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =           { "http://localhost:5001/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:5001/index.html" },
                    AllowedCorsOrigins =     { "http://localhost:5001" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        config.Scope
                    }
                }
            };
    }
}
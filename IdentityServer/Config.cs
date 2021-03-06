﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                new IdentityResource[]
                   {
                        new IdentityResources.OpenId(),
                        new IdentityResources.Profile(),
                        new IdentityResource()
                        {
                             Name = JwtClaimTypes.Role,
                             UserClaims = new List<string>(){"role"}
                        }
                   };
        public static IEnumerable<ApiResource> ApiResources =>
         new[]
            {
                new ApiResource
                {
                    Name = "omsApi",
                    DisplayName = "OMS API",
                    Description = "Allow the application to access OMSApi on your behalf",
                    Scopes = new List<string> {"omsApi.categories", "omsApi.products"},
                    UserClaims = new List<string> { JwtClaimTypes.Role }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(name : "omsApi.categories", displayName : "Reads categories"),
                new ApiScope(name : "omsApi.products", displayName : "Reads products")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = {"openid", "profile", JwtClaimTypes.Role, "omsApi.products", "omsApi.categories"}
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    //ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://www.getpostman.com/ouath2/callback" },
                    AlwaysIncludeUserClaimsInIdToken =true,
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "omsApi.categories" }
                },
            };
    }
}
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VetClinic.IdentityServer.Models;

namespace VetClinic.IdentityServer.Configurations
{
    public static class Config
    {
        public static List<User> GetUsers =>
            new List<User>
            {
                new User
                {
                    Id = "1",
                    Email = "base@gmail.com",
                    PhoneNumber = "+380504443322"

                }
            };         

        public static List<TestUser> TestUsers =>
           new List<TestUser>
           {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "Ivanov@gmail.com",
                    Password = "qwerty",
                    Claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, "Client")
                        
                    }
                }
           };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API"),

            };

        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                //new ApiResource("api1", "API #1") {Scopes = {"api1"}},
                new ApiResource
                {
                    Name = "api1",
                    DisplayName = "API #1",
                    Scopes = {"api1"},
                    UserClaims =
                    {
                        JwtClaimTypes.Role
                    }
                }
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = JwtClaimTypes.Role,
                    UserClaims =
                    {
                        JwtClaimTypes.Role
                    }
                }
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"api1"}
                },

                new Client
                {
                    ClientId = "swagger3",
                    ClientName = "Swagger UI for demo_api",
                    ClientSecrets = {new Secret("secret".Sha256())}, // change me!
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    //RedirectUris = {"https://localhost:5001/swagger/oauth2-redirect.html"},
                    AllowedCorsOrigins = {"https://localhost:5001"},
                    AllowedScopes = {"api1"}
                },

                new Client
                {
                    ClientId = "swagger",
                    //ClientName = "swagger",
                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    //AllowedGrantTypes = GrantTypes.Code,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedCorsOrigins =
                    {
                        "https://localhost:5001",
                        "https://localhost:44308"
                    },

                    AllowedScopes =
                    {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        JwtClaimTypes.Role
                    },

                    //RequirePkce = true,
                    //AllowPlainTextPkce = false
                },
                new Client
                {
                    ClientId = "swagger2",
                    //ClientName = "swagger2",
                    RequireClientSecret = false,
                    

                    //AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    AllowedGrantTypes = GrantTypes.Code,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                  

                    AllowedScopes =
                    {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true
                },
            };
    }
}

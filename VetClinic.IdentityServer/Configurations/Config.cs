using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace VetClinic.IdentityServer.Configurations
{
    public static class Config
    {        
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
                        new Claim("RoleType", "Client")
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
                    ClientId = "swagger",
                    
                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedCorsOrigins =
                    {
                        "https://localhost:5001",
                        "https://localhost:44350"
                    },

                    AllowedScopes =
                    {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        JwtClaimTypes.Role
                    },
                }, 
            };
    }
}

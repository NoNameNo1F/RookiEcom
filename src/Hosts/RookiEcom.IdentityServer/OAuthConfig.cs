using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace RookiEcom.IdentityServer;

public class OAuthConfig
{
    public static IEnumerable<Client> Clients => new[]
    {
        new Client
        {
            ClientId = "rookiecom-frontstore",
            ClientName = "RookiEcom FrontStore",
            ClientSecrets = { new Secret("Rookie".Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { "https://localhost:5001/signin-oidc" },
            PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
            AllowedScopes = { OpenIdConnectScope.OpenId, OpenIdConnectScope.Profile, "rookiecom-webapi", "role" },
            RequireConsent = false,
            AllowOfflineAccess = true,
            AlwaysIncludeUserClaimsInIdToken = true,
            UpdateAccessTokenClaimsOnRefresh = true,
            AccessTokenType = AccessTokenType.Jwt,
            AllowedCorsOrigins = { "https://localhost:5001"},
            AlwaysSendClientClaims = true,
            ClientClaimsPrefix = "",
            CoordinateLifetimeWithUserSession = false
        },
        new Client
        {
            ClientId = "rookiecom-backoffice",
            ClientName = "RookiEcom BackOffice",
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            RequireClientSecret = false,
            RedirectUris = { "https://localhost:3000/signin-oidc" },
            PostLogoutRedirectUris = { "https://localhost:3000/logout-callback" },
            AllowedCorsOrigins = { "https://localhost:3000" },
            AllowedScopes = { OpenIdConnectScope.OpenId, OpenIdConnectScope.Profile, "rookiecom-webapi", "role" },
            AllowOfflineAccess = true,
            AccessTokenType = AccessTokenType.Jwt,
            CoordinateLifetimeWithUserSession = false,
            AlwaysIncludeUserClaimsInIdToken = false
        }
    };

    public static IEnumerable<ApiScope> ApiScopes => new[]
    {
        new ApiScope
        {
            Name = "rookiecom-webapi",
            DisplayName = "RookiEcom WebAPI",
        },
        new ApiScope
        {
            Name = "role",
            DisplayName = "Role"
        }
    };
    public static IEnumerable<ApiResource> ApiResources => new[]
    {
        new ApiResource("rookiecom-webapi", "RookiEcom WebAPI")
        {
            Scopes = { "rookiecom-webapi" }
        }
    };
    public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email(),
    };
}
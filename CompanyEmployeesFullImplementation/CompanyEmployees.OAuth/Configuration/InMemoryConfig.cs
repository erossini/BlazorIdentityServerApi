using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace CompanyEmployees.OAuth.Configuration
{
	public static class InMemoryConfig
	{
		public static IEnumerable<IdentityResource> GetIdentityResources() =>
		  new List<IdentityResource>
		  {
			  new IdentityResources.OpenId(),
			  new IdentityResources.Profile(),
			  new IdentityResources.Address(),
			  new IdentityResource("roles", "User role(s)", new List<string> { "role" }),
			  new IdentityResource("position", "Your position", new List<string> { "position" }),
			  new IdentityResource("country", "Your country", new List<string> { "country" })
		  };

		public static IEnumerable<ApiScope> GetApiScopes() =>
		   new List<ApiScope> { new ApiScope("companyApi", "CompanyEmployee API") };

		public static IEnumerable<ApiResource> GetApiResources() =>
			new List<ApiResource>
			{
				new ApiResource("companyApi", "CompanyEmployee API")
				{
					Scopes = { "companyApi" }
				}
			};

		public static List<TestUser> GetUsers() =>
		  new List<TestUser>
		  {
			  new TestUser
			  {
				  SubjectId = "a9ea0f25-b964-409f-bcce-c923266249b4",
				  Username = "Enrico",
				  Password = "EnricoPassword",
				  Claims = new List<Claim>
				  {
					  new Claim(JwtClaimTypes.Name, "Enrico Rossini"),
					  new Claim("given_name", "Enrico"),
					  new Claim("family_name", "Rossini"),
					  new Claim("address", "London Street 4"),
					  new Claim("role", "Admin"),
					  new Claim("position", "Administrator"),
					  new Claim("country", "UK")
				  }
			  },
			  new TestUser
			  {
				  SubjectId = "c95ddb8c-79ec-488a-a485-fe57a1462340",
				  Username = "Test",
				  Password = "TestPassword",
				  Claims = new List<Claim>
				  {
					  new Claim(JwtClaimTypes.Name, "Test User"),
					  new Claim("given_name", "Test"),
					  new Claim("family_name", "User"),
					  new Claim("address", "London Avenue 289"),
					  new Claim("role", "Visitor"),
					  new Claim("position", "Viewer"),
					  new Claim("country", "UK")
				  }
			  }
		  };

		public static IEnumerable<Client> GetClients() =>
			new List<Client>
			{
			   new Client
			   {
					ClientId = "company-employee",
					ClientSecrets = new [] { new Secret("puresourcecode".Sha512()) },
					AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
					AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "companyApi" }
			   },
			   new Client
			   {
				   ClientName = "MVC Client",
				   ClientId = "mvc-client",
				   AllowedGrantTypes = GrantTypes.Hybrid,
				   RedirectUris = new List<string>{ "https://localhost:5010/signin-oidc" },
				   RequirePkce = false,
				   AllowedScopes =
				   {
					   IdentityServerConstants.StandardScopes.OpenId,
					   IdentityServerConstants.StandardScopes.Profile,
					   IdentityServerConstants.StandardScopes.Address,
					   "roles",
					   "companyApi",
					   "position",
					   "country"
				   },
				   ClientSecrets = { new Secret("MVCSecret".Sha512()) },
				   PostLogoutRedirectUris = new List<string> { "https://localhost:5010/signout-callback-oidc" },
				   RequireConsent = true
			   },
			   new Client
			   {
				   ClientId = "blazorWASM",
				   AllowedGrantTypes = GrantTypes.Code,
				   RequirePkce = true,
				   RequireClientSecret = false,
				   AllowedCorsOrigins = { "https://localhost:5020" },
				   AllowedScopes =
				   {
					   IdentityServerConstants.StandardScopes.OpenId,
					   IdentityServerConstants.StandardScopes.Profile
				   },
				   RedirectUris = { "https://localhost:5020/authentication/login-callback" },
				   PostLogoutRedirectUris = { "https://localhost:5020/authentication/logout-callback" }
			   }
			};
	}
}

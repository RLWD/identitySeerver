using IdentityServer4.Models;
using System.Collections.Generic;

namespace VDW.SalesApp.IdentityServer.Models
{
	public static class Config
	{
		public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope> {
			new ApiScope("salesappapis", "Sales App API Scope")
		};
		public static IEnumerable<Client> Clients => new List<Client> {
			new Client {
				ClientId = "SalesApp.Backoffice",
					AllowedGrantTypes = GrantTypes.ClientCredentials,
					ClientSecrets = {
						new Secret("salesappbosecret".Sha256())
					},
					AllowedScopes = {
						"salesappapis"
					}
			}
		};
	}
}

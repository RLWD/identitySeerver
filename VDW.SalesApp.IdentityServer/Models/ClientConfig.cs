using IdentityServer4.Models;
using System.Linq;

namespace VDW.SalesApp.IdentityServer.Models
{
	public class ClientConfig
	{
		public string ClientName { get; set; }
		public string ClientId { get; set; }
		public string[] ClientSecrets { get; set; }
		public string[] Scopes { get; set; }

		public Client ToInMemoryClient()
		{
			return new Client()
			{
				Enabled = true,
				ClientName = ClientName,
				ClientId = ClientId,
				AllowedGrantTypes = GrantTypes.ClientCredentials,
				ClientSecrets = ClientSecrets.Select(s => { return new Secret(s.Sha256()); }).ToList(),
				AllowedScopes = Scopes,
				RefreshTokenExpiration = TokenExpiration.Sliding
			};
		}
	}
}
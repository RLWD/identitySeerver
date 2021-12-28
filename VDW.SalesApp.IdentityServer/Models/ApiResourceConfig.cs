using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;

namespace VDW.SalesApp.IdentityServer.Models
{
	public class ApiResourceConfig
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public string[] ApiSecrets { get; set; }

		public ApiResource ToInMemoryApiResource()
		{
			return new ApiResource(Name, DisplayName)
			{
				ApiSecrets = ApiSecrets.Select(s => { return new Secret(s.Sha256()); }).ToList(),
				Scopes = new List<string>() { Name }
			};
		}
	}
}
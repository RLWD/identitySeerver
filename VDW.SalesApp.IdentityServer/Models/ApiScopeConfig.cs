using IdentityServer4.Models;

namespace VDW.SalesApp.IdentityServer.Models
{
	public class ApiScopeConfig
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }

		public ApiScope ToInMemoryApiScope()
		{
			return new ApiScope(Name, DisplayName);
		}
	}
}
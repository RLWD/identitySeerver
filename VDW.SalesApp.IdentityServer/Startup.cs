using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using VDW.SalesApp.IdentityServer.Models;

namespace VDW.SalesApp.IdentityServer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			var inMemoryApiScopes = Configuration.GetSection("ApiScopes").Get<List<ApiScopeConfig>>().Select(c => c.ToInMemoryApiScope());
			var inMemoryClients = Configuration.GetSection("Clients").Get<List<ClientConfig>>().Select(c => c.ToInMemoryClient());

			_ = services.AddIdentityServer()
						.AddDeveloperSigningCredential()
						.AddInMemoryApiScopes(inMemoryApiScopes)
						.AddInMemoryClients(inMemoryClients);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseDeveloperExceptionPage();
			app.UseIdentityServer();
		}
	}
}

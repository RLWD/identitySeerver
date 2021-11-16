using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VDW.SalesApp.IdentityServer.Models;
using VDW.SalesApp.IdentityServer.Services;

namespace VDW.SalesApp.IdentityServer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			_ = services.AddIdentityServer()
						.AddDeveloperSigningCredential()
						.AddInMemoryApiScopes(Config.ApiScopes)
						.AddInMemoryClients(Config.Clients)
						.AddProfileService<ProfileService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseDeveloperExceptionPage();
			app.UseIdentityServer();
		}
	}
}

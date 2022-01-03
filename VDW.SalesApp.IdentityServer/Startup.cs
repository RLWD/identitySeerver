using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using VDW.SalesApp.IdentityServer.Certificates;
using VDW.SalesApp.IdentityServer.Models;

namespace VDW.SalesApp.IdentityServer
{
	public class Startup
	{
		public IConfiguration _configuration { get; }
		private IWebHostEnvironment _environment { get; }

		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			_configuration = configuration;
			_environment = env;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			var inMemoryApiScopes = _configuration.GetSection("ApiScopes").Get<List<ApiScopeConfig>>().Select(c => c.ToInMemoryApiScope());
			var inMemoryApiResources = _configuration.GetSection("ApiResources").Get<List<ApiResourceConfig>>().Select(c => c.ToInMemoryApiResource());
			var inMemoryClients = _configuration.GetSection("Clients").Get<List<ClientConfig>>().Select(c => c.ToInMemoryClient());

			var idsvrBuilder = services.AddIdentityServer();

			//if (_environment.IsDevelopment())
			//{
			//	idsvrBuilder.AddDeveloperSigningCredential();
			//}
			//else
			//{
				var (ActiveCertificate, SecondaryCertificate) = GetCertificates(_environment, _configuration).GetAwaiter().GetResult();
				idsvrBuilder.AddSigningCredential(ActiveCertificate);
			//}

			idsvrBuilder
				.AddInMemoryApiScopes(inMemoryApiScopes)
				.AddInMemoryApiResources(inMemoryApiResources)
				.AddInMemoryClients(inMemoryClients);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseDeveloperExceptionPage();
			app.UseIdentityServer();
		}

		private static async Task<(X509Certificate2 ActiveCertificate, X509Certificate2 SecondaryCertificate)> GetCertificates(IWebHostEnvironment environment, IConfiguration configuration)
		{
			var certificateConfig = new CertificateConfig
			{
				// Use an Azure key vault
				KeyVaultCertificateName = configuration["Certificate:AzureKeyVaultCertificateName"],
				KeyVaultEndpoint = $"https://{configuration["Certificate:AzureKeyVaultName"]}.vault.azure.net/",
			};

			(X509Certificate2 ActiveCertificate, X509Certificate2 SecondaryCertificate) certs = (null, null);

			if (!string.IsNullOrEmpty(certificateConfig.KeyVaultEndpoint))
			{
				var keyVaultCertificateService = new KeyVaultCertificateService(certificateConfig.KeyVaultEndpoint, certificateConfig.KeyVaultCertificateName);
				certs = await keyVaultCertificateService.GetCertificatesFromKeyVault().ConfigureAwait(false);
			}

			return certs;
		}
	}
}

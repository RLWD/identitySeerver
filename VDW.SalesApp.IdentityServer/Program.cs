using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;

namespace VDW.SalesApp.IdentityServer
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((context, config) =>
				 {
					 var builder = config.Build();

					 var keyVaultName = builder["Certificate:AzureKeyVaultName"];
					 if (!string.IsNullOrEmpty(keyVaultName))
					 {
						 var keyVaultEndpoint = $"https://{keyVaultName}.vault.azure.net/";
						 var azureServiceTokenProvider = new AzureServiceTokenProvider();
						 var keyVaultClient = new KeyVaultClient(
							 new KeyVaultClient.AuthenticationCallback(
								 azureServiceTokenProvider.KeyVaultTokenCallback));

						 config.AddAzureKeyVault(
							 keyVaultEndpoint,
							 keyVaultClient,
							 new DefaultKeyVaultSecretManager());
					 }
				 })
				.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
	}
}

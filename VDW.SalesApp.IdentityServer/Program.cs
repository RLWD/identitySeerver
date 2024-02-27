using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

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

                     if (context.HostingEnvironment.IsProduction())
                     {
                         var builtConfig = config.Build();
                         var keyVaultName = builder["Certificate:AzureKeyVaultName"];
                         var secretClient = new SecretClient(
                             new Uri($"https://{keyVaultName}.vault.azure.cn/"),
                             new DefaultAzureCredential());
                         config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
                     }
                 })
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}

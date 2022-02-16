using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace SalesApp.Ocelot.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseContentRoot(Directory.GetCurrentDirectory())
                      .ConfigureAppConfiguration((builderContext, config) =>
                      {
                          var env = builderContext.HostingEnvironment;
                          config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                          config.AddEnvironmentVariables();
                      });
                });
    }
}

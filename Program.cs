

namespace Galvanize.Gateway
{
	using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;
	using System.IO;

	public class Program
	{
		public static void Main(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{Environments.Development}.json", optional: true)
				.Build();

			BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
				 .UseContentRoot(Directory.GetCurrentDirectory())
		  .ConfigureAppConfiguration((hostingContext, config) =>
		  {
			  config
				  .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
				  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				  .AddJsonFile($"appsettings.{Environments.Development}.json", optional: true)
				  .AddJsonFile("ocelot.Production.json")
				  .AddEnvironmentVariables();
		  })
		  .ConfigureServices(s =>
		  {
		  })
		   .ConfigureLogging((hostingContext, logging) =>
		   {
			   logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
			   logging.AddConsole();
		   })
		  .UseIISIntegration()
		  .Configure(app =>
		  {
			  // app.UseOcelot().Wait();
		  })
		  .UseStartup<Startup>()
		  .Build();
		}
	}
}

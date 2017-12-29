using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore;
using NLog.Web;
using NLog.Extensions.Logging;
using System;

namespace Narato.Libraries.POC.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // setup the logger first to catch all errors
            var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                BuildWebHost(args).Run();
            }
            catch (Exception e)
            {
                logger.Error(e, "Stopped program because of exception");
                throw;
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // delete all default configuration providers
                    config.Sources.Clear();

                    var env = hostingContext.HostingEnvironment;
                    config.SetBasePath(env.ContentRootPath)
                    .AddJsonFile("hosting.json", optional: true)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddJsonFile("config.json")
                    .AddJsonFile("config.json.local", optional: true)
                    .AddEnvironmentVariables();

                    env.ConfigureNLog("nlog.config");
                })
                .UseStartup<Startup>()
                .UseNLog()
                .Build();
        }
    }
}

﻿using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog.Enrichers.AspnetcoreHttpcontext;
using Serilog;
using Common.Serilog;
using Scheduler.API;

namespace Spider.Scheduler
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        public static int Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Host terminated unexpectedly");
                Console.Write(ex.ToString());
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var settings = Configuration.GetSection("ScheduleSettings").Get<ScheduleSettings>();

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((provider, context, loggerConfig) =>
                {
                    loggerConfig.WithSimpleConfiguration(provider, settings.ApplicationName, Configuration);
                });
        }
    }
}

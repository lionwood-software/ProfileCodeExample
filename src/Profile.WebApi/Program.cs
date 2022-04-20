using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Profile.WebApi.Extensions;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Profile.WebApi
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return -1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddAzureKeyVaultSupport();
                })
                .UseSerilog((hostBuilderContext, services, loggerConfiguration) =>
                {
                    if (hostBuilderContext.HostingEnvironment.IsDevelopment())
                    {
                        loggerConfiguration.WriteTo.Seq("http://localhost:5341");
                    }

                    loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration);
                    if (hostBuilderContext.HostingEnvironment.IsProduction())
                    {
                        loggerConfiguration.AddApplicationInsightsLogging(hostBuilderContext.Configuration, services);
                    }
                }, preserveStaticLogger: true)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}

using System.Collections.Generic;
using System.IO;
using FluentValidation.AspNetCore;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Adapters;
using Profile.Adapters.Authorization;
using Profile.Adapters.Extensions;
using Profile.Adapters.Options;
using Profile.Core.Extensions;
using Profile.Core.Options;
using Profile.Core.SharedKernel;
using Serilog;
using Serilog.Events;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Profile.Adapters
{
    public class Startup : FunctionsStartup
    {
        public IConfiguration Configuration { get; set; }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var context = builder.GetContext();
            var isDevelopment = context.EnvironmentName == "Development";

            Configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
                .AddAzureKeyVaultSupport()
                .AddEnvironmentVariables()
                .Build();

            var services = builder.Services;
            RegisterCore(services);
            AddLogging(services, isDevelopment);
        }

        private void RegisterCore(IServiceCollection services)
        {
            services.AddCoreInfrastructure(
                options => options.SupportedLanguages = Configuration.GetSection(SupportedLanguagesOptions.SectionName).Get<List<string>>());

            services.AddFluentValidation(fv =>
            {
                fv.ImplicitlyValidateChildProperties = true;
                fv.ImplicitlyValidateRootCollectionElements = true;
                fv.RegisterValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            });

            services.AddScoped<IProfileUserContext, FunctionUserContext>();

            services.Configure<DatabaseOptions>(Configuration.GetSection(DatabaseOptions.SectionName));
            var databaseOptions = Configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>();
            services.AddSqlDatabase(databaseOptions);
        }

        private void AddLogging(IServiceCollection services, bool isDevelopment)
        {
            services.Configure<ApplicationInsightsOptions>(Configuration.GetSection(ApplicationInsightsOptions.SectionName));
            var loggerConfiguration = new LoggerConfiguration();
            if (!isDevelopment)
            {
                var applicationInsightsOptions = Configuration.GetSection(ApplicationInsightsOptions.SectionName).Get<ApplicationInsightsOptions>();
                var options = new ApplicationInsightsServiceOptions { ConnectionString = applicationInsightsOptions.ConnectionString };
                services.AddApplicationInsightsTelemetry(options: options);

                loggerConfiguration.WriteTo.ApplicationInsights(
                    TelemetryConfiguration.CreateDefault(), TelemetryConverter.Traces, LogEventLevel.Information);
            }
            else
            {
                loggerConfiguration.WriteTo.Seq("http://localhost:5342");
            }

            var logger = loggerConfiguration
                .WriteTo.Console(LogEventLevel.Information)
                .Enrich.WithProperty("ApplicationName", "Profile.Function")
                .Destructure.ToMaximumCollectionCount(10)
                .Destructure.ToMaximumDepth(4)
                .CreateLogger();

            services.AddLogging(c => c.AddSerilog(logger));
        }
    }
}

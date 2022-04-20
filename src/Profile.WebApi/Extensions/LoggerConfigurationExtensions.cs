using System;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Profile.WebApi.Extensions
{
    internal static class LoggerConfigurationExtensions
    {
        internal static LoggerConfiguration AddApplicationInsightsLogging(this LoggerConfiguration loggerConfiguration, IConfiguration configuration, IServiceProvider services)
        {
            loggerConfiguration.ReadFrom.Configuration(configuration)
                .WriteTo.ApplicationInsights(services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces, LogEventLevel.Information)
                    .Enrich.FromLogContext();

            return loggerConfiguration;
        }
    }
}
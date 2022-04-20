using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Profile.WebApi.Extensions;
using Profile.WebApi.HealthChecks;

namespace Profile.WebApi.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder builder)
        {
            builder.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    var response = new HealthCheckResponse
                    {
                        Status = report.Status,
                        HealthChecks = report.Entries.Select(x => new IndividualHealthCheckResponse
                        {
                            Component = x.Key,
                            Status = x.Value.Status,
                        }).ToList(),
                        HealthCheckDuration = report.TotalDuration
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new StringEnumConverter()));
                }
            });

            return builder;
        }
    }
}
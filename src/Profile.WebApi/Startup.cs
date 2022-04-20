using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Profile.Core;
using Profile.Core.Extensions;
using Profile.Core.Options;
using Profile.Core.SharedKernel;
using Profile.Infrastructure.Extensions;
using Profile.Infrastructure.Matching.Migration;
using Profile.Infrastructure.ServiceBus.Options;
using Profile.WebApi.Extensions;
using Profile.WebApi.Infrastructure;
using Profile.WebApi.Localization;
using Profile.WebApi.Middleware;
using Profile.WebApi.Options;
using Profile.WebApi.Security;
using Serilog;

namespace Profile.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddSwaggerGenWithAuthorization();

            if (Environment.IsProduction())
            {
                var applicationInsightsOptions = Configuration.GetSection(ApplicationInsightsOptions.SectionName).Get<ApplicationInsightsOptions>();
                if (!string.IsNullOrEmpty(applicationInsightsOptions.ConnectionString))
                {
                    services.AddApplicationInsightsTelemetry(Configuration);
                    services.AddSingleton<ITelemetryInitializer, ProfileServiceTelemetryInitializer>();
                }
            }

            RegisterCore(services);
            RegisterEventBus(services);
            RegisterFileManager(services);
            AddAuthentication(services);
            RegisterCorsPolicy(services);

            services.AddLocalization();

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy("healthy"))
                .AddDbContextCheck<ProfileDbContext>("database", HealthStatus.Unhealthy);
        }

        private void AddAuthentication(IServiceCollection services)
        {
            //var options = Configuration.GetSection("Auth").Get<AuthOptions>();
            //services.AddAuthentication(options);
            services.AddAuthorization(o =>
            {
                //o.AddMaintenancePolicy();
                o.AddPolicy(IndividualAuthPolicy.Individual, builder => builder.AddRequirements(new IndividualScopeRequirement()));
            });
            //services.AddPolicyHandlers();
            services.AddSingleton<IAuthorizationHandler, IndividualScopeRequirementHandler>();
        }

        private void RegisterCore(IServiceCollection services)
        {
            services.AddCoreInfrastructure(
                options => options.SupportedLanguages = Configuration.GetSection(SupportedLanguagesOptions.SectionName).Get<List<string>>());

            services.AddFluentValidation(fv =>
            {
                fv.ImplicitlyValidateChildProperties = true;
                fv.ImplicitlyValidateRootCollectionElements = true;
                fv.RegisterValidatorsFromAssembly(typeof(Core.Extensions.ServiceCollectionExtensions).Assembly);
            });
            services.AddScoped<ProfileUserContext>();
            services.AddScoped<IProfileUserContext, ProfileUserContext>(p => p.GetRequiredService<ProfileUserContext>());

            services.AddScoped<ProfilesMigrator>();

            services.Configure<DatabaseOptions>(Configuration.GetSection(DatabaseOptions.SectionName));
            var databaseOptions = Configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>();
            services.AddSqlDatabase(databaseOptions);
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            services.Configure<ServiceBusOptions>(Configuration.GetSection(ServiceBusOptions.SectionName));

            if (Environment.IsDevelopment())
            {
                services.AddLocalServiceBusInfrastructure();
            }

            if (Environment.IsProduction())
            {
                var serviceBusOptions = Configuration.GetSection(ServiceBusOptions.SectionName).Get<ServiceBusOptions>();
                services.AddServiceBusInfrastructure(serviceBusOptions);
            }
        }

        private void RegisterFileManager(IServiceCollection services)
        {
            services.Configure<AzureBlobOptions>(Configuration.GetSection(AzureBlobOptions.SectionName));
            var azureBlobOptions = Configuration.GetSection(AzureBlobOptions.SectionName).Get<AzureBlobOptions>();
            services.AddAzureBlobInfrastructure(azureBlobOptions);
        }

        private void RegisterCorsPolicy(IServiceCollection services)
        {
            var corsOptions = Configuration.GetSection(CorsOptions.SectionName).Get<CorsOptions>();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(corsOptions.AllowedUrls)
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithMethods(corsOptions.AllowedMethods);
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (Configuration.GetValue<bool>("ExposeSwagger"))
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Profile.WebApi v1"));
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            if (env.IsProduction())
            {
                app.UseCors();
            }
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseProfileRequestLocalization();

            app.UseHealthChecks();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

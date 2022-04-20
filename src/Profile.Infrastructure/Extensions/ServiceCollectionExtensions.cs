using System;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Profile.Core.EventBus;
using Profile.Core.FileManager;
using Profile.Core.Options;
using Profile.Infrastructure.ServiceBus;
using Profile.Infrastructure.ServiceBus.Options;

namespace Profile.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBusInfrastructure(this IServiceCollection services, ServiceBusOptions serviceBusOptions)
        {
            services.AddAzureClients(azureClientFactoryBuilder =>
            {
                azureClientFactoryBuilder.AddServiceBusClient(serviceBusOptions.ConnectionString)
                .ConfigureOptions(options =>
                {
                    options.RetryOptions.Delay = TimeSpan.FromMilliseconds(serviceBusOptions.RetryOptions.Delay);
                    options.RetryOptions.MaxDelay = TimeSpan.FromSeconds(serviceBusOptions.RetryOptions.MaxDelay);
                    options.RetryOptions.MaxRetries = serviceBusOptions.RetryOptions.MaxRetries;
                });
            });
            services.AddSingleton<UserCreatedSubscription>();
            services.AddHostedService<ServiceBusWorker>();
            services.AddSingleton<IServiceBusClient, ServiceBusClient>();
            services.AddScoped<IEventBusPort, ServiceBusAdapter>();
            return services;
        }

        public static IServiceCollection AddLocalServiceBusInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IServiceBusClient, LocalServiceBusClient>();
            services.AddScoped<IEventBusPort, ServiceBusAdapter>();
            return services;
        }

        public static IServiceCollection AddAzureBlobInfrastructure(this IServiceCollection services, AzureBlobOptions azureBlobOptions)
        {
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(azureBlobOptions.ConnectionString);
            });

            services.AddScoped<IAvatarStorage, AvatarStorage>();

            return services;
        }
    }
}
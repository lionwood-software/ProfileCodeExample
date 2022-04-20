using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Profile.Infrastructure.ServiceBus
{
    public sealed class ServiceBusWorker : IHostedService, IAsyncDisposable
    {
        private readonly ILogger<ServiceBusWorker> logger;
        private readonly UserCreatedSubscription userCreatedSubscription;

        public ServiceBusWorker(ILogger<ServiceBusWorker> logger,
            UserCreatedSubscription userCreatedSubscription)
        {
            this.logger = logger;
            this.userCreatedSubscription = userCreatedSubscription;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting the service bus user created subscription.");
            await userCreatedSubscription.RegisterMessageHandler();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stopping the service bus user created subscription.");
            await userCreatedSubscription.CloseSubscriptionAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await userCreatedSubscription.DisposeAsync();
        }
    }
}
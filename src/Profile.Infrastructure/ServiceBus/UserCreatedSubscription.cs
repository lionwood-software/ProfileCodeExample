using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Profile.Core.Extensions;
using Profile.Core.Options;
using Profile.Core.SharedKernel;
using Profile.Core.Users.CreateUser;
using Profile.Infrastructure.ServiceBus.Options;
using Profile.Infrastructure.ServiceBus.Schemas;

namespace Profile.Infrastructure.ServiceBus
{
    public class UserCreatedSubscription
    {
        private readonly Azure.Messaging.ServiceBus.ServiceBusClient serviceBusClient;
        private readonly ILogger<UserCreatedSubscription> logger;
        private readonly IConfiguration configuration;
        private readonly ServiceBusOptions serviceBusOptions;
        private readonly DatabaseOptions databaseOptions;
        private readonly ServiceProvider serviceProvider;
        private ServiceBusProcessor processor;

        public UserCreatedSubscription(
            Azure.Messaging.ServiceBus.ServiceBusClient serviceBusClient,
            ILogger<UserCreatedSubscription> logger,
            IOptions<ServiceBusOptions> serviceBusOptions,
            IOptions<DatabaseOptions> databaseOptions,
            IConfiguration configuration)
        {
            this.serviceBusClient = serviceBusClient;
            this.logger = logger;
            this.serviceBusOptions = serviceBusOptions.Value;
            this.databaseOptions = databaseOptions.Value;
            this.configuration = configuration;
            serviceProvider = BuildServiceProvider();
        }

        public async Task RegisterMessageHandler()
        {
            processor = serviceBusClient.CreateProcessor(
                serviceBusOptions.Topics.IndividualEvents,
                serviceBusOptions.Subscriptions.UserCreated,
                new ServiceBusProcessorOptions
                {
                    MaxConcurrentCalls = 1,
                    AutoCompleteMessages = false
                });

            processor.ProcessMessageAsync += ProcessMessagesAsync;
            processor.ProcessErrorAsync += ProcessErrorAsync;

            await processor.StartProcessingAsync();
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            if (Enum.TryParse(args.Message.Subject, true, out EventType eventType))
            {
                using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

                switch (eventType)
                {
                    case EventType.UserCreated:
                        {
                            try
                            {
                                var user = JsonConvert.DeserializeObject<UserCreatedSchema>(args.Message.Body.ToString());
                                var handler = scope.ServiceProvider.GetRequiredService<CreateUserHandler>();
                                await handler.Handle(new CreateUserRecordCommand
                                {
                                    Email = user.Email,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    LanguageCode = user.LanguageCode,
                                    UserId = user.UserId,
                                });
                                break;
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "Failed to handle message: {message}", args.Message.Body);
                                throw;
                            }
                        }
                }
                await args.CompleteMessageAsync(args.Message);
            }
            else
            {
                logger.LogWarning("An error occurred while processing a message. System has received subject - {subject} which is not recognizable by application.", args.Message.Subject);
            }
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            logger.LogError(arg.Exception,
                "Error during handling message: {exception}" +
                " - Context: {context}" +
                " - Entity Path: {path}", arg.Exception.Message, arg.ErrorSource, arg.EntityPath);

            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (processor != null)
            {
                await processor.DisposeAsync();
            }
        }

        public async Task CloseSubscriptionAsync()
        {
            await processor.CloseAsync();
        }

        private ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddCoreInfrastructure(
                options => options.SupportedLanguages = configuration.GetSection(SupportedLanguagesOptions.SectionName).Get<List<string>>());
            services.AddScoped<IProfileUserContext, MaintenanceProfileUserContext>();
            services.AddSqlDatabase(databaseOptions);
            services.AddLogging();

            return services.BuildServiceProvider();
        }
    }
}
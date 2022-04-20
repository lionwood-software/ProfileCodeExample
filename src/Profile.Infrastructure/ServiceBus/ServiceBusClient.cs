using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Profile.Infrastructure.ServiceBus.Options;

namespace Profile.Infrastructure.ServiceBus
{
    public class ServiceBusClient : IServiceBusClient
    {
        private readonly Azure.Messaging.ServiceBus.ServiceBusClient serviceBusClient;
        private static Dictionary<string, Lazy<ServiceBusSender>> senders;

        public ServiceBusClient(
            Azure.Messaging.ServiceBus.ServiceBusClient serviceBusClient,
            IOptions<ServiceBusOptions> serviceBusOptions)
        {
            this.serviceBusClient = serviceBusClient;
            senders = new()
            {
                { serviceBusOptions.Value.Topics.ProfileEvents, CreateLazySender(serviceBusOptions.Value.Topics.ProfileEvents) },
                { serviceBusOptions.Value.Topics.IndividualEvents, CreateLazySender(serviceBusOptions.Value.Topics.IndividualEvents) },
                { serviceBusOptions.Value.Topics.SyncProfiles, CreateLazySender(serviceBusOptions.Value.Topics.SyncProfiles) },
                { serviceBusOptions.Value.Topics.MatchingProfileUpdates, CreateLazySender(serviceBusOptions.Value.Topics.MatchingProfileUpdates) },
            };
        }

        private Lazy<ServiceBusSender> CreateLazySender(string topic)
        {
            return new Lazy<ServiceBusSender>(() => serviceBusClient.CreateSender(topic));
        }

        public async Task SendMessage(object message, string type, string topic)
        {
            var serviceBusSender = senders[topic].Value;
            var body = JsonConvert.SerializeObject(message);
            var serviceBusMessage = new ServiceBusMessage(body) { Subject = type };
            await serviceBusSender.SendMessageAsync(serviceBusMessage);
        }
    }
}
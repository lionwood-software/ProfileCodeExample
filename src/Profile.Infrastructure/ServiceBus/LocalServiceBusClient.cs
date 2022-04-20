using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Profile.Infrastructure.ServiceBus
{
    public class LocalServiceBusClient : IServiceBusClient
    {
        private readonly ILogger<LocalServiceBusClient> logger;
        public LocalServiceBusClient(ILogger<LocalServiceBusClient> logger) { this.logger = logger; }
        public Task SendMessage(object message, string type, string topic)
        {
            logger.LogInformation("Event {type} for topic {topic} have been published: {event}", type, topic, JsonConvert.SerializeObject(message));
            return Task.CompletedTask;
        }
    }
}
using System.Threading.Tasks;

namespace Profile.Infrastructure.ServiceBus
{
    public interface IServiceBusClient
    {
        Task SendMessage(object message, string type, string topic);
    }
}
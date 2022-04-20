using System;
using System.Threading.Tasks;

namespace Profile.Core.EventBus
{
    public interface IEventBusPort
    {
        Task Publish(Guid userId, ProfileEventType type);
    }
}

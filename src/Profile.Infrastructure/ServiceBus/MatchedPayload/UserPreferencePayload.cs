using System.Collections.Generic;

namespace Profile.Infrastructure.ServiceBus.MatchedPayload
{
    public record UserPreferencePayload
    {
        public int CategoryId { get; init; }
        public IEnumerable<int> RolesIds { get; init; }
        public bool WillDo { get; init; }
        public bool CanDo { get; init; }
    }
}
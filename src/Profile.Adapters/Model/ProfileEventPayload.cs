using System;

namespace Profile.Adapters.Model
{
    public class ProfileEventPayload
    {
        public Guid UserId { get; set; }

        public int NeoId { get; set; }
    }
}

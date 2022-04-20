using System;

namespace Profile.Infrastructure.ServiceBus
{
    public class BirthDataPayload
    {
        public DateTime? BirthDate { get; set; }
        public int? CountryId { get; set; }
        public int? RegionId { get; set; }
        public string Place { get; set; }
    }
}

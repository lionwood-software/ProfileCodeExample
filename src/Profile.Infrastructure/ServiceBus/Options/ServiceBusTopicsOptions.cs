namespace Profile.Infrastructure.ServiceBus.Options
{
    public class ServiceBusTopicsOptions
    {
        public const string SectionName = "Topics";

        public string IndividualEvents { get; set; }
        public string ProfileEvents { get; set; }
        public string MatchingProfileUpdates { get; set; }
        public string SyncProfiles { get; set; }
    }
}
namespace Profile.Infrastructure.ServiceBus.Options
{
    public class ServiceBusOptions
    {
        public const string SectionName = "ServiceBus";

        public string ConnectionString { get; set; }
        public ServiceBusRetryOptions RetryOptions { get; set; }
        public ServiceBusTopicsOptions Topics { get; set; }
        public ServiceBusSubscriptionsOptions Subscriptions { get; set; }
    }
}
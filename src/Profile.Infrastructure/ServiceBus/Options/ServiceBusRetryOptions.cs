namespace Profile.Infrastructure.ServiceBus.Options
{
    public class ServiceBusRetryOptions
    {
        public int Delay { get; set; }
        public int MaxDelay { get; set; }
        public int MaxRetries { get; set; }
    }
}

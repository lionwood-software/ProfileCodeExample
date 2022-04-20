namespace Profile.Adapters.Options
{
    public class ApplicationInsightsOptions
    {
        public const string SectionName = "ApplicationInsights";

        public string ConnectionString { get; set; }
    }
}
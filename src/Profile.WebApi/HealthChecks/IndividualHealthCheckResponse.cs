using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Profile.WebApi.HealthChecks
{
    public class IndividualHealthCheckResponse
    {
        public HealthStatus Status { get; set; }
        public string Component { get; set; }
    }
}
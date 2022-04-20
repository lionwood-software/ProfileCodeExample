using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Profile.WebApi.Infrastructure
{
    internal sealed class ProfileServiceTelemetryInitializer : ITelemetryInitializer
    {
        private const string ApplicationName = "ProfileService";
        public void Initialize(ITelemetry telemetry)
        {
            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            {
                telemetry.Context.Cloud.RoleName = ApplicationName;
                telemetry.Context.Cloud.RoleInstance = ApplicationName;
            }
        }
    }
}
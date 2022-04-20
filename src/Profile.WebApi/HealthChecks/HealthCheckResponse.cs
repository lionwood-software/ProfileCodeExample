using System;
using System.Collections.Generic;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Profile.WebApi.HealthChecks
{
    public class HealthCheckResponse
    {
        public HealthStatus Status { get; set; }
        public List<IndividualHealthCheckResponse> HealthChecks { get; set; }
        public TimeSpan HealthCheckDuration { get; set; }
    }
}
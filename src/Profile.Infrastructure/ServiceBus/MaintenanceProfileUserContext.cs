using System;
using System.Globalization;
using Profile.Core.SharedKernel;

namespace Profile.Infrastructure.ServiceBus
{
    public class MaintenanceProfileUserContext : IProfileUserContext
    {
        public Guid UserId { get; set; }
        public string Email => "profileservicesystem@mail.com";
        public bool HasUserId() => false;
        public void SetCulture(string culture)
        {
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
        }

        /// <summary>
        /// Sets to constant value for maintenance - user cookies are not available
        /// </summary>
        public string Culture => "en";
    }
}

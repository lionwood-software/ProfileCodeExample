using System.Globalization;
using Profile.Core.SharedKernel;

namespace Profile.Migrator
{
    public class MigrationUserContext : IProfileUserContext
    {
        public Guid UserId { get; set; }
        public string Email => "profileservicemigration@mail.com";
        public bool HasUserId() => false;
        public void SetCulture(string culture)
        {
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
        }

        /// <summary>
        /// Sets to constant during migration changes.
        /// </summary>
        public string Culture => "en";
    }
}
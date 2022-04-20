using System.Collections.Generic;

namespace Profile.Core.Users.PersonalInfo.Summary
{
    public class UserSummary
    {
        public string AvatarUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Language { get; set; }
        public string PhoneNumber { get; set; }
        public string Nir { get; set; }
        public IEnumerable<UserLanguageProficiencySummary> LanguageProficiencies { get; set; }
        public IEnumerable<UserExtraCitySummary> ExtraCities { get; set; }
        public IEnumerable<UserExtraPreferenceSummary> ExtraPreferences { get; set; }
        public UserBirthDataSummary UserBirthData { get; set; }
        public IEnumerable<UserCountryWorkPermitSummary> UserCountryWorkPermits { get; set; }
    }
}
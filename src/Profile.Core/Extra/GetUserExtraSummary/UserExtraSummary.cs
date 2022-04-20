using System;
using System.Collections.Generic;
using Profile.Core.Users.PersonalInfo;

namespace Profile.Core.Extra.GetUserExtraSummary
{
    public record UserExtraSummary
    {
        public Guid UserId { get; init; }
        public IEnumerable<int> CityIds { get; set; }
        public string LanguageCode { get; set; }
        public IEnumerable<LanguageProficiency> LanguageProficiencies { get; set; }
        public IEnumerable<UserExtraPreference> ExtraPreferences { get; set; }
        public string Nir { get; set; }
        public UserExtraBirthDataSummary UserBirthData { get; set; }
        public IEnumerable<UserExtraCountryWorkPermitSummary> UserCountryWorkPermits { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace Profile.Infrastructure.ServiceBus.MatchedPayload
{
    public record UserSummaryPayload
    {
        public Guid UserId { get; init; }
        public string LanguageCode { get; set; }
        public IEnumerable<int> CityIds { get; set; }
        public IEnumerable<UserLanguageProficiencyPayload> LanguageProficiencies { get; set; }
        public IEnumerable<UserPreferencePayload> ExtraPreferences { get; set; }
    }
}
using System;

namespace Profile.Infrastructure.ServiceBus
{
    public class LanguageProficiencyPayload
    {
        public string LanguageCode { get; set; }
        public int ProficiencyId { get; set; }
    }
}

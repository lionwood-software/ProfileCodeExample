using System;
using System.Collections.Generic;
using Profile.Infrastructure.ServiceBus;

namespace Profile.Infrastructure.ServiceBus
{
    public class UserPayload
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string LanguageCode { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public string Nir { get; set; }
        public int? NeoId { get; set; }
        public List<LanguageProficiencyPayload> LanguageProficiencies { get; set; }
        public BirthDataPayload BirthData { get; set; }
    }
}

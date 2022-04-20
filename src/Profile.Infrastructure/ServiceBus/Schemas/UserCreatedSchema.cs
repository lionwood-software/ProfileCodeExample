using System;

namespace Profile.Infrastructure.ServiceBus.Schemas
{
    public class UserCreatedSchema
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LanguageCode { get; set; }
    }
}
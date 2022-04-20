using System;

namespace Profile.Infrastructure.ServiceBus.MatchedPayload;
public class UserSyncPayload
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string LanguageCode { get; set; }
    public string PhoneNumber { get; set; }
    public string AvatarUrl { get; set; }
}
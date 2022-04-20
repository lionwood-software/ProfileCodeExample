using System;

namespace Profile.Core.Users.GetUserInfo
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LanguageCode { get; set; }
        public bool IsOnboarded { get; set; }
        public string AvatarUrl { get; set; }
    }
}

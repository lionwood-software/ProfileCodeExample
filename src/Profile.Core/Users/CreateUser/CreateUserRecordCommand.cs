using System;

namespace Profile.Core.Users.CreateUser
{
    public record CreateUserRecordCommand
    {
        public Guid UserId { get; init; }
        public string Email { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string LanguageCode { get; init; }
    }
}

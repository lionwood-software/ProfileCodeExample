using System;
using Microsoft.AspNetCore.Http;

namespace Profile.Core
{
    public class UserContext : IUserContext
    {
        public UserContext(IHttpContextAccessor httpContextAccessor) { }

        public virtual Guid UserId { get; }
    }
}

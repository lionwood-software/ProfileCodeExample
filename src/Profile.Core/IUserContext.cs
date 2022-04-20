using System;

namespace Profile.Core
{
    public interface IUserContext
    {
        Guid UserId { get; }
    }
}

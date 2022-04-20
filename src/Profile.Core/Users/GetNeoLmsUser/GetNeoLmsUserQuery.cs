using System;
using MediatR;

namespace Profile.Core.Users.GetNeoLmsUser
{
    public class GetNeoLmsUserQuery : IRequest<NeoLmsUserInfo>
    {
        public Guid UserId { get; set; }
    }
}

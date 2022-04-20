using System;
using MediatR;

namespace Profile.Core.Users.PersonalInfo.UpdateNeoLmsUser
{
    public class UpdateNeoIdCommand : IRequest
    {
        public Guid UserId { get; set; }

        public int NeoId { get; set; }
    }
}

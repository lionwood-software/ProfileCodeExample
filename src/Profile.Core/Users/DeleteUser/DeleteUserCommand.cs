using MediatR;
using System;

namespace Profile.Core.Users.DeleteUser
{
    public class DeleteUserCommand : IRequest
    {
        public Guid UserId { get; set; }
    }
}

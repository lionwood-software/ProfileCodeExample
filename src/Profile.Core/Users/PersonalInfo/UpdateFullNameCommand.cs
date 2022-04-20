using MediatR;

namespace Profile.Core.Users.PersonalInfo
{
    public class UpdateFullNameCommand : IRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}

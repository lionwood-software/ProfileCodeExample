using MediatR;

namespace Profile.Core.Users.PersonalInfo
{
    public class UpdatePhoneNumberCommand : IRequest
    {
        public string PhoneNumber { get; set; }
    }
}

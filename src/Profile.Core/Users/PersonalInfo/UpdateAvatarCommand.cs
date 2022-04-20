using MediatR;

namespace Profile.Core.Users.PersonalInfo
{
    public class UpdateAvatarCommand : IRequest
    {
        public string Content { get; set; }

        public string FileName { get; set; }
    }
}

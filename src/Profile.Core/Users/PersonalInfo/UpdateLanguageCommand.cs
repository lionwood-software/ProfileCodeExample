using MediatR;

namespace Profile.Core.Users.PersonalInfo
{
    public class UpdateLanguageCommand : IRequest<string>
    {
        public string LanguageCode { get; set; }
    }
}

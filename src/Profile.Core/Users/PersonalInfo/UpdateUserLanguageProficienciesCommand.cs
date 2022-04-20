using System.Collections.Generic;
using MediatR;

namespace Profile.Core.Users.PersonalInfo
{
    public class UpdateUserLanguageProficienciesCommand : IRequest
    {
        public List<LanguageProficiency> LanguageProficiencies { get; set; }
    }
}

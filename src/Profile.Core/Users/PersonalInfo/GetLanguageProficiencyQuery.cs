using System.Collections.Generic;
using MediatR;

namespace Profile.Core.Users.PersonalInfo
{
    public class GetLanguageProficiencyQuery : IRequest<List<LanguageProficiency>>
    {
    }
}

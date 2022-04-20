using System.Collections.Generic;
using MediatR;

namespace Profile.Core.Languages.GetLanguages
{
    public class GetLanguagesQuery : IRequest<List<Language>>
    {
    }
}

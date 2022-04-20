using System.Collections.Generic;
using MediatR;

namespace Profile.Core.Languages.GetSupportedLanguages
{
    public class GetSupportedLanguagesQuery : IRequest<List<SupportedLanguage>>
    {
    }
}

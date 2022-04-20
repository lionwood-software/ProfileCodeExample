using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Profile.Core.Options;
using Profile.Core.SharedKernel;

namespace Profile.Core.Languages.GetSupportedLanguages
{
    public class GetSupportedLanguagesQueryHandler : IRequestHandler<GetSupportedLanguagesQuery, List<SupportedLanguage>>
    {
        private readonly SupportedLanguagesOptions supportedLanguagesOptions;
        private readonly IProfileUserContext userContext;

        public GetSupportedLanguagesQueryHandler(
            IOptions<SupportedLanguagesOptions> options,
            IProfileUserContext userContext)
        {
            supportedLanguagesOptions = options.Value;
            this.userContext = userContext;
        }

        public async Task<List<SupportedLanguage>> Handle(GetSupportedLanguagesQuery request, CancellationToken cancellationToken) =>
            LanguagesHelper.GetSupportedLanguages()
            .Where(x => supportedLanguagesOptions.SupportedLanguages.Contains(x.Key))
            .Select(x => new SupportedLanguage { Code = x.Key, Name = x.Value.FirstOrDefault(c => c.Culture == userContext.Culture).Name })
            .ToList();
    }
}
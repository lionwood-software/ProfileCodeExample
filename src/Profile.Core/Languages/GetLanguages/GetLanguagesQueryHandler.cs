using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Profile.Core.Languages.GetLanguages
{
    public class GetLanguagesQueryHandler : IRequestHandler<GetLanguagesQuery, List<Language>>
    {
        private readonly ProfileDbContext dbContext;
        public GetLanguagesQueryHandler(ProfileDbContext dbContext) { this.dbContext = dbContext; }

        public async Task<List<Language>> Handle(GetLanguagesQuery request, CancellationToken cancellationToken)
            => await dbContext.Languages.Select(x => new Language { Code = x.Code, Name = x.Translations.FirstOrDefault().Name, }).ToListAsync(cancellationToken);
    }
}
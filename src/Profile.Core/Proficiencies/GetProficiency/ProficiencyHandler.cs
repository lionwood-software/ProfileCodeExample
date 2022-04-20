using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile.Core.SharedKernel;

namespace Profile.Core.Proficiencies.GetProficiency
{
    public class ProficiencyHandler : IRequestHandler<GetProficienciesQuery, List<ListItem>>
    {
        private readonly ProfileDbContext dbContext;

        public ProficiencyHandler(ProfileDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<ListItem>> Handle(GetProficienciesQuery request, CancellationToken cancellationToken)
        => await dbContext.Proficiencies
            .Select(x => new ListItem
            {
                Key = x.Id,
                Value = x.Translations.FirstOrDefault().Name
            })
            .ToListAsync(cancellationToken);
    }
}

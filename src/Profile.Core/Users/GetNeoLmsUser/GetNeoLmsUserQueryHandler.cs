using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile.Core.Extensions;

namespace Profile.Core.Users.GetNeoLmsUser
{
    public class GetNeoLmsUserQueryHandler : IRequestHandler<GetNeoLmsUserQuery, NeoLmsUserInfo>
    {
        private readonly ProfileDbContext dbContext;

        public GetNeoLmsUserQueryHandler(ProfileDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<NeoLmsUserInfo> Handle(GetNeoLmsUserQuery request, CancellationToken cancellationToken)
            => await dbContext.Users
            .Where(x => x.Id == request.UserId)
            .Select(user => new NeoLmsUserInfo
            {
                Id = user.Id,
                NeoId = user.NeoId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                LanguageCode = user.LanguageCode,
                IsOnboarded = user.IsOnboarded,
                AvatarUrl = user.AvatarUrl
            })
            .SingleOrDefaultAsync(cancellationToken)
            .ThrowIfNotFound();
    }
}

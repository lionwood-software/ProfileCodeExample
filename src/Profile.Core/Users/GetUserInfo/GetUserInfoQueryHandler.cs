using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile.Core.Extensions;
using Profile.Core.SharedKernel;

namespace Profile.Core.Users.GetUserInfo
{
    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserInfo>
    {
        private readonly ProfileDbContext dbContext;
        private readonly IProfileUserContext profileUserContext;

        public GetUserInfoQueryHandler(IProfileUserContext profileUserContext, ProfileDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.profileUserContext = profileUserContext;
        }

        public async Task<UserInfo> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
            => await dbContext.Users
            .Where(x => x.Id == profileUserContext.UserId)
            .Select(user => new UserInfo
            {
                Id = user.Id,
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

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile.Core.SharedKernel;

namespace Profile.Core.Users.PersonalInfo.GetUserBirthData
{
    public class GetUserBirthDataQueryHandler : IRequestHandler<GetUserBirthDataQuery, UserBirthData>
    {
        private readonly ProfileDbContext dbContext;
        private readonly IProfileUserContext profileUserContext;

        public GetUserBirthDataQueryHandler(IProfileUserContext profileUserContext, ProfileDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.profileUserContext = profileUserContext;
        }

        public async Task<UserBirthData> Handle(GetUserBirthDataQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.UserBirthData.SingleOrDefaultAsync(x => x.UserId == profileUserContext.UserId, cancellationToken);
        }
    }
}

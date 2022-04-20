using MediatR;
using Profile.Core.Users.PersonalInfo.Summary;

namespace Profile.Core.Users.PersonalInfo
{
    public class GetUserSummaryQuery : IRequest<UserSummary>
    {
    }
}

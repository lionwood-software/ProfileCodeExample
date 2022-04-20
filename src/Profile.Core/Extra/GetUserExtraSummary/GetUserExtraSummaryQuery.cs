using System;
using MediatR;

namespace Profile.Core.Extra.GetUserExtraSummary
{
    public class GetUserExtraSummaryQuery : IRequest<UserExtraSummary>
    {
        public Guid? UserId { get; set; }
    }
}

using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Profile.Core.Matching.Extra
{
    public class GetFeaturedOpportunitiesHandler : IRequestHandler<GetFeaturedOpportunitiesQuery, FeaturedOpportunitiesResult>
    {
        private readonly IMatchingRequirementsChecker matchingRequirementsChecker;

        public GetFeaturedOpportunitiesHandler(IMatchingRequirementsChecker checker)
        {
            matchingRequirementsChecker = checker;
        }

        public async Task<FeaturedOpportunitiesResult> Handle(GetFeaturedOpportunitiesQuery request, CancellationToken cancellationToken)
        {
            var matchingFailureResult = await matchingRequirementsChecker.Check(cancellationToken);
            return new FeaturedOpportunitiesResult
            {
                Failure = matchingFailureResult,
            };
        }
    }
}
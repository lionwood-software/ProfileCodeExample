using System;
using System.Collections.Generic;

namespace Profile.Core.Matching.Extra
{
    public class FeaturedOpportunitiesResult : IHasMatchingFailure
    {
        public IEnumerable<ExtraOpportunity> Opportunities { get; set; } = Array.Empty<ExtraOpportunity>();
        public MatchingFailure Failure { get; set; }
    }
}
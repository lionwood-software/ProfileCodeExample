using System.Collections.Generic;
using Profile.Core.Matching.Extra;

namespace Profile.Core.Matching
{
    public class MatchingFailure
    {
        public MatchingFailure(IEnumerable<MatchingRequirement> missedRequirements)
        {
            MissedRequirements = missedRequirements;
        }

        public IEnumerable<MatchingRequirement> MissedRequirements { get; }
    }
}
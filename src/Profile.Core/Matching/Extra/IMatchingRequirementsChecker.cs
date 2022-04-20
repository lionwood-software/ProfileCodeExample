using System.Threading;
using System.Threading.Tasks;

namespace Profile.Core.Matching.Extra
{
    public interface IMatchingRequirementsChecker
    {
        Task<MatchingFailure> Check(CancellationToken cancellationToken = default);
    }
}
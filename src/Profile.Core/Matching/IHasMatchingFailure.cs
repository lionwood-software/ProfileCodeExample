namespace Profile.Core.Matching
{
    public interface IHasMatchingFailure
    {
        public MatchingFailure Failure { get; set; }
    }
}
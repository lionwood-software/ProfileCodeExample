namespace Profile.Core.Extra.UpdateUserPreferences
{
    public record UpdateUserPreferenceRecord
    {
        public int JobCategoryId { get; init; }
        public bool WillDo { get; init; }
        public bool CanDo { get; init; }
    }
}

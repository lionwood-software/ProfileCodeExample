namespace Profile.Infrastructure.ServiceBus.MatchedPayload
{
    public record UserLanguageProficiencyPayload
    {
        public string Code { get; init; }
        public int LevelId { get; init; }
    }
}
namespace Profile.Core.Users.PersonalInfo
{
    public record LanguageProficiency
    {
        public string LanguageCode { get; init; }
        public int ProficiencyLevel { get; init; }
    }
}

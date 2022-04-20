namespace Profile.Adapters.Options
{
    public class AzureKeyVaultOptions
    {
        public const string SectionName = "AzureKeyVault";

        public string Uri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Prefix { get; set; }
    }
}
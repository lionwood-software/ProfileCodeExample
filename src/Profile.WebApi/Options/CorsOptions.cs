namespace Profile.WebApi.Options
{
    public class CorsOptions
    {
        public const string SectionName = "Cors";

        public string[] AllowedUrls { get; set; }
        public string[] AllowedMethods { get; set; }
    }
}

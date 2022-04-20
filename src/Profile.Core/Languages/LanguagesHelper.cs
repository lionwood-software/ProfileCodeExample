using System.Collections.Generic;

namespace Profile.Core.Languages
{
    internal static class LanguagesHelper
    {
        public static Dictionary<string, List<LanguageTranslation>> GetSupportedLanguages() =>
            new()
            {
                {
                    "en",
                    new List<LanguageTranslation>()
                    {
                        new() { Culture = "en", Name = "English" },
                        new() { Culture = "fr", Name = "anglais" },
                        new() { Culture = "de", Name = "Englisch" },
                    }
                },
                {
                    "fr",
                    new List<LanguageTranslation>()
                    {
                        new() { Culture = "en", Name = "French" },
                        new() { Culture = "fr", Name = "français" },
                        new() { Culture = "de", Name = "Französisch" },
                    }
                },
                {
                    "de",
                    new List<LanguageTranslation>()
                    {
                        new() { Culture = "en", Name = "German" },
                        new() { Culture = "fr", Name = "allemand" },
                        new() { Culture = "de", Name = "Deutsch" },
                    }
                }
            };
    }
}

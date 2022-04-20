using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Profile.Core;
using Profile.Core.Languages;

namespace Profile.Migrator
{
    public class LanguagesMigrator
    {
        public static async Task Migrate(ProfileDbContext dbContext)
        {
            if (await dbContext.Languages.AnyAsync())
            {
                Console.WriteLine("Languages with translations are already populated. Skipping.");
                return;
            }

            var languageRecords = await FileExtensions.ReadJson<Collection<LanguageRecord>>("resources/languages.json");
            var languageTranslations = new[]
            {
                new LanguageTranslationResource { Culture = "en", Translation = await ReadLanguageTranslations("resources/languages.en.json") },
                new LanguageTranslationResource { Culture = "fr", Translation = await ReadLanguageTranslations("resources/languages.fr.json") },
                new LanguageTranslationResource { Culture = "de", Translation = await ReadLanguageTranslations("resources/languages.de.json") }
            };

            var languages = languageRecords.Select(x => MatToLanguageEntity(x, languageTranslations)).ToList();

            dbContext.Languages.AddRange(languages);

            await dbContext.SaveChangesAsync();
        }

        private static Language MatToLanguageEntity(LanguageRecord record, LanguageTranslationResource[] translations)
            => new()
            {
                Code = record.Code,
                Name = record.Name,
                Translations = translations.Select(translation => new LanguageTranslation
                {
                    Culture = translation.Culture,
                    Name = translation.Translation[record.Code].Name
                }).ToList()
            };

        private static async Task<Dictionary<string, LanguageTranslationRecord>> ReadLanguageTranslations(string resourcePath)
        {
            var languageTranslations = await FileExtensions.ReadJson<List<LanguageTranslationRecord>>(resourcePath);
            return languageTranslations.ToDictionary(x => x.Code, x => x);
        }

        private record LanguageRecord
        {
            [JsonPropertyName("name")]
            public string Name { get; init; }

            [JsonPropertyName("code")]
            public string Code { get; init; }

            [JsonPropertyName("alternativeSpellingEN")]
            public string AlternativeSpellingEN { get; init; }

            [JsonPropertyName("alternativeSpellingFR")]
            public string AlternativeSpellingFR { get; init; }
        }

        private record LanguageTranslationRecord
        {
            [JsonPropertyName("name")]
            public string Name { get; init; }

            [JsonPropertyName("code")]
            public string Code { get; init; }
        }

        private class LanguageTranslationResource
        {
            public string Culture { get; set; } = null!;
            public IDictionary<string, LanguageTranslationRecord> Translation { get; set; } = null!;
        }
    }
}

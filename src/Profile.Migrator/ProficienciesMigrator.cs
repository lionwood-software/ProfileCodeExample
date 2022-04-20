using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Profile.Core;
using Profile.Core.Proficiencies;

namespace Profile.Migrator
{
    public class ProficienciesMigrator
    {
        public static async Task Migrate(ProfileDbContext dbContext)
        {
            if (await dbContext.Proficiencies.AnyAsync())
            {
                Console.WriteLine("Proficiencies are already populated. Skipping.");
                return;
            }

            var proficiencies = await FileExtensions.ReadJson<IEnumerable<ProficiencyRecord>>("resources/proficiencies.json");
            var translations = new[]
            {
                new ProficiencyTranslationResource { Culture = "de", Translation = await ReadTranslations("resources/proficiencies.de.json") },
                new ProficiencyTranslationResource { Culture = "en", Translation = await ReadTranslations("resources/proficiencies.en.json") },
                new ProficiencyTranslationResource { Culture = "fr", Translation = await ReadTranslations("resources/proficiencies.fr.json") }
            };

            var profileProficiencies = MapToProficiency(proficiencies, translations);
            dbContext.AddRange(profileProficiencies);

            await dbContext.EnableIdentityInsert<Proficiency>();
            await dbContext.SaveChangesAsync();
            await dbContext.DisableIdentityInsert<Proficiency>();
        }

        private static IEnumerable<Proficiency> MapToProficiency(
            IEnumerable<ProficiencyRecord> proficiencies,
            IEnumerable<ProficiencyTranslationResource> translations)
        {
            return proficiencies.ToList().Select(proficiency =>
            {
                return new Proficiency
                {
                    Id = proficiency.Id,
                    Name = proficiency.Name,
                    Translations = translations.Select(t => new ProficiencyTranslation
                    {
                        Culture = t.Culture,
                        Name = t.Translation[proficiency.Id].Name
                    }).ToList()
                };
            });
        }

        private static async Task<Dictionary<int, ProficiencyTranslationRecord>> ReadTranslations(string resourcePath)
        {
            var translations = await FileExtensions.ReadJson<List<ProficiencyTranslationRecord>>(resourcePath);
            return translations.OrderBy(s => s.Id).ToDictionary(x => x.Id, x => x);
        }

        private class ProficiencyTranslationResource
        {
            public string Culture { get; set; } = null!;
            public IDictionary<int, ProficiencyTranslationRecord> Translation { get; set; } = null!;
        }

        private class ProficiencyTranslationRecord
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; } = null!;
        }

        private class ProficiencyRecord
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; } = null!;
        }
    }
}
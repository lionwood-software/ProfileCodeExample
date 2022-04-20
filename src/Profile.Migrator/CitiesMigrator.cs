using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Profile.Core;
using Profile.Core.Extra;
using Profile.Core.Locations;

namespace Profile.Migrator
{
    public static class CitiesMigrator
    {
        public static async Task Migrate(ProfileDbContext dbContext)
        {
            if (await dbContext.Cities.AnyAsync())
            {
                Console.WriteLine("Cities are already populated. Skipping.");
                return;
            }

            var cities = await FileExtensions.ReadJson<Collection<CityRecord>>("resources/cities.json");
            var countries = await dbContext.Countries.Select(x => new { x.Id, x.Alpha2 }).ToDictionaryAsync(x => x.Alpha2, x => x.Id);
            var translations = new[]
            {
                new CityTranslationResource { Culture = "de", Translation = await ReadTranslations("resources/cities.de.json") },
                new CityTranslationResource { Culture = "en", Translation = await ReadTranslations("resources/cities.en.json") },
                new CityTranslationResource { Culture = "fr", Translation = await ReadTranslations("resources/cities.fr.json") }
            };

            var profileCities = cities.Select(x => MatToCity(x, countries, translations));

            dbContext.Cities.AddRange(profileCities);

            await dbContext.ResetIdentityCount<City>();
            await dbContext.EnableIdentityInsert<City>();
            await dbContext.SaveChangesAsync();
            await dbContext.DisableIdentityInsert<City>();
        }

        private static City MatToCity(CityRecord x, Dictionary<string, int> countries, CityTranslationResource[] translations)
        {
            return new()
            {
                Id = x.Id,
                GeonamesId = x.GeonamesId,
                Name = x.Name,
                AsciiName = x.AsciiName,
                AlternateNames = x.AlternateNames,
                CountryId = countries[x.CountryCode.ToLowerInvariant()],
                Timezone = x.Timezone,
                Translations = translations.Select(t => new CityTranslation
                {
                    Culture = t.Culture,
                    Name = t.Translation[x.GeonamesId].Name
                }).ToList()
            };
        }

        public static async Task MigrateActiveCities(ProfileDbContext dbContext)
        {
            if (await dbContext.ActiveCities.AnyAsync())
            {
                Console.WriteLine("Active cities are already populated. Skipping.");
                return;
            }

            var cityIds = await FileExtensions.ReadJson<Collection<int>>("resources/activeCities.json");

            var activeCities = cityIds.Select(id => new ActiveCity { Id = id }).ToList();
            dbContext.ActiveCities.AddRange(activeCities);

            await dbContext.SaveChangesAsync();
        }

        private static async Task<Dictionary<string, CityTranslationRecord>> ReadTranslations(string resourcePath)
        {
            var translations = await FileExtensions.ReadJson<List<CityTranslationRecord>>(resourcePath);
            return translations.ToDictionary(x => x.Id, x => x);
        }

        private class CityTranslationResource
        {
            public string Culture { get; set; } = null!;
            public IDictionary<string, CityTranslationRecord> Translation { get; set; } = null!;
        }

        private class CityTranslationRecord
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }
        }

        private class CityRecord
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("geonamesId")]
            public string GeonamesId { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; } = null!;

            [JsonPropertyName("asciiName")]
            public string AsciiName { get; set; } = null!;

            [JsonPropertyName("alternateNames")]
            public string AlternateNames { get; set; } = null!;

            [JsonPropertyName("countryCode")]
            public string CountryCode { get; set; } = null!;

            [JsonPropertyName("timezone")]
            public string Timezone { get; set; } = null!;
        }
    }
}
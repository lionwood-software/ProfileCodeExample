using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Profile.Core;
using Profile.Core.Extra;
using Profile.Core.Locations;

namespace Profile.Migrator
{
    public class CountriesMigrator
    {
        public static async Task Migrate(ProfileDbContext dbContext)
        {
            if (await dbContext.Countries.AnyAsync())
            {
                Console.WriteLine("Countries are already populated. Skipping.");
                return;
            }

            var primary = await ReadCountries("resources/countries.en.json");
            var translations = new[]
            {
                new CountryTranslationResource { Culture = "de", Translation = await ReadCountries("resources/countries.de.json") },
                new CountryTranslationResource { Culture = "en", Translation = await ReadCountries("resources/countries.en.json") },
                new CountryTranslationResource { Culture = "fr", Translation = await ReadCountries("resources/countries.fr.json") }
            };

            var profileCountries = MapToProfileCountries(primary, translations);
            dbContext.Countries.AddRange(profileCountries);

            await dbContext.ResetIdentityCount<Country>();
            await dbContext.EnableIdentityInsert<Country>();
            await dbContext.SaveChangesAsync();
            await dbContext.DisableIdentityInsert<Country>();
        }

        public static async Task MigrateActiveCountries(ProfileDbContext dbContext)
        {
            if (await dbContext.ActiveCountries.AnyAsync())
            {
                Console.WriteLine("Active countries are already populated. Skipping.");
                return;
            }

            var activeCountryRecords = await FileExtensions.ReadJson<List<ActiveCountryRecord>>("resources/activeCountries.json");
            var countries = await dbContext.Countries.ToListAsync();
            var activeCountries = activeCountryRecords.Select(x => new ActiveCountry
            {
                Id = countries.Single(c => c.Alpha2 == x.CountryCode.ToLowerInvariant()).Id,
                ItemOrder = x.ItemOrder
            }).ToList();

            dbContext.ActiveCountries.AddRange(activeCountries);

            await dbContext.SaveChangesAsync();
        }

        private static IEnumerable<Country> MapToProfileCountries(Dictionary<string, CountryRecord> enCountries, CountryTranslationResource[] translations)
        {
            return enCountries.ToList().OrderBy(x => x.Value.Name).Select(keyValuePair =>
            {
                var country = keyValuePair.Value;
                return new Country
                {
                    Id = country.Id,
                    Name = country.Name,
                    Alpha2 = country.Alpha2,
                    Alpha3 = country.Alpha3,
                    Translations = translations.Select(x => new CountryTranslation
                    {
                        Culture = x.Culture,
                        Name = x.Translation[keyValuePair.Key].Name
                    }).ToList()
                };
            });
        }

        private static async Task<Dictionary<string, CountryRecord>> ReadCountries(string resourcePath)
        {
            var listOfCountries = await FileExtensions.ReadJson<List<CountryRecord>>(resourcePath);
            return listOfCountries.ToDictionary(x => x.Alpha3, x => x);
        }

        private class CountryTranslationResource
        {
            public string Culture { get; set; } = null!;
            public IDictionary<string, CountryRecord> Translation { get; set; } = null!;
        }

        private class CountryRecord
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; } = null!;

            [JsonPropertyName("alpha2")]
            public string Alpha2 { get; set; } = null!;

            [JsonPropertyName("alpha3")]
            public string Alpha3 { get; set; } = null!;
        }

        private class ActiveCountryRecord
        {
            [JsonPropertyName("countryCode")]
            public string CountryCode { get; set; }

            [JsonPropertyName("itemOrder")]
            public int ItemOrder { get; set; }
        }
    }
}
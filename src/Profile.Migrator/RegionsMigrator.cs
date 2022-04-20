using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Profile.Core;
using Profile.Core.Locations;

namespace Profile.Migrator
{
    public static class RegionsMigrator
    {
        public static async Task Migrate(ProfileDbContext dbContext)
        {
            if (await dbContext.Regions.AnyAsync())
            {
                Console.WriteLine("Regions are already populated. Skipping.");
                return;
            }

            var regions = await FileExtensions.ReadJson<Collection<RegionRecord>>("resources/regions.json");
            var countries = await dbContext.Countries.Select(x => new { x.Id, x.Alpha2 }).ToDictionaryAsync(x => x.Alpha2, x => x.Id);

            var profileRegions = regions.Select(x => MatToCity(x, countries));

            dbContext.Regions.AddRange(profileRegions);

            await dbContext.ResetIdentityCount<Region>();
            await dbContext.EnableIdentityInsert<Region>();
            await dbContext.SaveChangesAsync();
            await dbContext.DisableIdentityInsert<Region>();
        }

        private static Region MatToCity(RegionRecord x, Dictionary<string, int> countries)
        {
            return new()
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                CountryId = countries[x.CountryCode.ToLowerInvariant()],
            };
        }

        private class RegionRecord
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; } = null!;

            [JsonPropertyName("code")]
            public string Code { get; set; } = null!;

            [JsonPropertyName("countryCode")]
            public string CountryCode { get; set; } = null!;
        }
    }
}

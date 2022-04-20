using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Profile.Core;
using Profile.Core.Extra;

namespace Profile.Migrator
{
    public static class JobCategoriesMigrator
    {
        public static async Task Migrate(ProfileDbContext dbContext)
        {
            if (await dbContext.JobCategories.AnyAsync())
            {
                Console.WriteLine("JobCategories and JobRoles are already populated. Skipping.");
                return;
            }

            var categories = await FileExtensions.ReadJson<Collection<CategoryRecord>>("resources/jobCategories.json");
            var categoryTranslations = new[]
            {
                new TranslationResource<CategoryTranslationRecord> { Culture = "en", Translation = await ReadJobCategoryTranslations("resources/jobCategories.en.json") },
                new TranslationResource<CategoryTranslationRecord> { Culture = "fr", Translation = await ReadJobCategoryTranslations("resources/jobCategories.fr.json") },
                new TranslationResource<CategoryTranslationRecord> { Culture = "de", Translation = await ReadJobCategoryTranslations("resources/jobCategories.de.json") }
            };

            var roles = await FileExtensions.ReadJson<Collection<RoleRecord>>("resources/jobRoles.json");
            var roleTranslations = new[]
            {
                new TranslationResource<RoleTranslationRecord> { Culture = "en", Translation = await ReadJobRoleTranslations("resources/jobRoles.en.json") },
                new TranslationResource<RoleTranslationRecord> { Culture = "fr", Translation = await ReadJobRoleTranslations("resources/jobRoles.fr.json") },
                new TranslationResource<RoleTranslationRecord> { Culture = "de", Translation = await ReadJobRoleTranslations("resources/jobRoles.de.json") }
            };

            var jobCategories = MapToEntities(categories, categoryTranslations);
            dbContext.JobCategories.AddRange(jobCategories);
            await dbContext.EnableIdentityInsert<JobCategory>();
            await dbContext.SaveChangesAsync();
            await dbContext.DisableIdentityInsert<JobCategory>();

            var jobRoles = MapToEntities(roles, roleTranslations);
            dbContext.JobRoles.AddRange(jobRoles);
            await dbContext.EnableIdentityInsert<JobRole>();
            await dbContext.SaveChangesAsync();
            await dbContext.DisableIdentityInsert<JobRole>();
        }

        private static IEnumerable<JobCategory> MapToEntities(
            IEnumerable<CategoryRecord> categories,
            IEnumerable<TranslationResource<CategoryTranslationRecord>> categoryTranslations)
                => categories.Select(cat => new JobCategory
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    Translations = categoryTranslations.Select(ct => new JobCategoryTranslation
                    {
                        Culture = ct.Culture,
                        Name = ct.Translation[cat.Id]?.Name
                    }).ToList(),
                }).ToList();

        private static IEnumerable<JobRole> MapToEntities(
            IEnumerable<RoleRecord> roles,
            IEnumerable<TranslationResource<RoleTranslationRecord>> roleTranslations)
                => roles.Select(role => new JobRole
                {
                    Id = role.Id,
                    Name = role.Name,
                    JobCategoryId = role.CategoryId,
                    Translations = roleTranslations.Select(rt => new JobRoleTranslation
                    {
                        Culture = rt.Culture,
                        Name = rt.Translation[role.Id]?.Name
                    }).ToList()
                }).ToList();

        private static async Task<Dictionary<int, CategoryTranslationRecord>> ReadJobCategoryTranslations(string resourcePath)
        {
            var jobCategoryTranslations = await FileExtensions.ReadJson<List<CategoryTranslationRecord>>(resourcePath);
            return jobCategoryTranslations.ToDictionary(x => x.CategoryId, x => x);
        }

        private static async Task<Dictionary<int, RoleTranslationRecord>> ReadJobRoleTranslations(string resourcePath)
        {
            var jobRoleTranslations = await FileExtensions.ReadJson<List<RoleTranslationRecord>>(resourcePath);
            return jobRoleTranslations.ToDictionary(x => x.RoleId, x => x);
        }

        private record CategoryRecord
        {
            [JsonPropertyName("name")]
            public string Name { get; init; }

            [JsonPropertyName("id")]
            public int Id { get; set; }
        }

        private record CategoryTranslationRecord
        {
            [JsonPropertyName("categoryId")]
            public int CategoryId { get; init; }

            [JsonPropertyName("name")]
            public string Name { get; init; }
        }

        private record RoleRecord
        {
            [JsonPropertyName("id")]
            public int Id { get; init; }

            [JsonPropertyName("name")]
            public string Name { get; init; }

            [JsonPropertyName("categoryId")]
            public int CategoryId { get; init; }
        }

        private record RoleTranslationRecord
        {
            [JsonPropertyName("roleId")]
            public int RoleId { get; init; }

            [JsonPropertyName("name")]
            public string Name { get; init; }
        }

        private class TranslationResource<T>
        {
            public string Culture { get; set; } = null!;
            public IDictionary<int, T> Translation { get; set; } = null!;
        }
    }
}

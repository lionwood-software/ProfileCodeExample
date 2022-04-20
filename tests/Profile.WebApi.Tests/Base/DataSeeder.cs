using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Profile.Adapters;
using Profile.Core;
using Profile.Core.Extra;
using Profile.Core.Proficiencies;
using Profile.Migrator;

namespace Profile.WebApi.Tests.Base
{
    public class DataSeeder
    {
        public static async Task Seed(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var dbOptions = provider.GetService<DbContextOptions<ProfileDbContext>>();

            using var dbContext = new ProfileDbContext(dbOptions, new MigrationUserContext());

            dbContext.Users.Add(ProfileApiFactory<Startup>.ExecutionUser);
            dbContext.Proficiencies.AddRange(new Proficiency[]
           {
                 new Proficiency
                 {
                     Id = 1,
                     Name = "Beginner",
                     Translations = new List<ProficiencyTranslation>
                     {
                         new ProficiencyTranslation { Culture = "en", Name = "Beginner (A1 - A2)" },
                         new ProficiencyTranslation { Culture = "fr", Name = "Débutant (A1 - A2)" },
                         new ProficiencyTranslation { Culture = "de", Name = "Anfänger (A1 - A2)" }
                     }
                 },
                 new Proficiency
                 {
                     Id = 2,
                     Name = "Intermediate",
                     Translations = new List<ProficiencyTranslation>
                     {
                         new ProficiencyTranslation { Culture = "en", Name = "Intermediate (B1 - B2)" },
                         new ProficiencyTranslation { Culture = "fr", Name = "Intermédiaire (B1 - B2)" },
                         new ProficiencyTranslation { Culture = "de", Name = "Mittelstufe (B1 - B2)" }
                     }
                 },
                 new Proficiency
                 {
                     Id = 3,
                     Name = "Advanced",
                     Translations = new List<ProficiencyTranslation>
                     {
                         new ProficiencyTranslation { Culture = "en", Name = "Advanced (C1)" },
                         new ProficiencyTranslation { Culture = "fr", Name = "Avancé (С1)" },
                         new ProficiencyTranslation { Culture = "de", Name = "Fortgeschritten (С1)" }
                     }
                 },
                 new Proficiency
                 {
                     Id = 4,
                     Name = "Native",
                     Translations = new List<ProficiencyTranslation>
                     {
                         new ProficiencyTranslation { Culture = "en", Name = "Native (C2 +)" },
                         new ProficiencyTranslation { Culture = "fr", Name = "Natif (С2 +)" },
                         new ProficiencyTranslation { Culture = "de", Name = "Muttersprache (С2 +)" }
                     }
                 }
           });
            dbContext.JobCategories.AddRange(new JobCategory[]
            {
                new JobCategory
                {
                    Id = 1,
                    Name = "Beverage Services",
                    Translations = new List<JobCategoryTranslation>
                    {
                        new JobCategoryTranslation { Culture = "en", Name = "Beverage Services" },
                        new JobCategoryTranslation { Culture = "fr", Name = "Beverage Services FR" },
                        new JobCategoryTranslation { Culture = "de", Name = "Beverage Services DE" },
                    },
                    JobRoles = new List<JobRole>
                    {
                        new JobRole
                        {
                            Name = "Bartender",
                            Translations = new List<JobRoleTranslation>
                            {
                                new JobRoleTranslation { Culture = "en", Name = "Bartender" },
                                new JobRoleTranslation { Culture = "fr", Name = "Barkeeper/in" },
                                new JobRoleTranslation { Culture = "de", Name = "Barman" },
                            }
                        },
                        new JobRole
                        {
                            Name = "Wine Server",
                            Translations = new List<JobRoleTranslation>
                            {
                                new JobRoleTranslation { Culture = "en", Name = "Wine Server" },
                                new JobRoleTranslation { Culture = "fr", Name = "Weinkellner/in" },
                                new JobRoleTranslation { Culture = "de", Name = "Commis sommelier" },
                            }
                        }
                    }
                },
                new JobCategory
                {
                    Id = 2,
                    Name = "Food and Beverage Service",
                    Translations = new List<JobCategoryTranslation>
                    {
                        new JobCategoryTranslation { Culture = "en", Name = "Food and Beverage Service" },
                        new JobCategoryTranslation { Culture = "fr", Name = "Food and Beverage Service FR" },
                        new JobCategoryTranslation { Culture = "de", Name = "Food and Beverage Service DE" },
                    },
                    JobRoles = new List<JobRole>
                    {
                        new JobRole
                        {
                            Name = "Server",
                            Translations = new List<JobRoleTranslation>
                            {
                                new JobRoleTranslation { Culture = "en", Name = "Server" },
                                new JobRoleTranslation { Culture = "fr", Name = "Kellner/in" },
                                new JobRoleTranslation { Culture = "de", Name = "Serveur" },
                            }
                        },
                        new JobRole
                        {
                            Name = "Host",
                            Translations = new List<JobRoleTranslation>
                            {
                                new JobRoleTranslation { Culture = "en", Name = "Host" },
                                new JobRoleTranslation { Culture = "fr", Name = "Host/Hostess" },
                                new JobRoleTranslation { Culture = "de", Name = "Chef de rang" },
                            }
                        }
                    }
                },
                new JobCategory
                {
                    Id = 3,
                    Name = "Pastry and Baking",
                    Translations = new List<JobCategoryTranslation>
                    {
                        new JobCategoryTranslation { Culture = "en", Name = "Pastry and Baking" },
                        new JobCategoryTranslation { Culture = "fr", Name = "Pastry and Baking FR" },
                        new JobCategoryTranslation { Culture = "de", Name = "Pastry and Baking DE" },
                    },
                    JobRoles = new List<JobRole>
                    {
                        new JobRole
                        {
                            Name = "Kitchen Assistant",
                            Translations = new List<JobRoleTranslation>
                            {
                                new JobRoleTranslation { Culture = "en", Name = "Kitchen Assistant" },
                                new JobRoleTranslation { Culture = "fr", Name = "Küchenhilfe" },
                                new JobRoleTranslation { Culture = "de", Name = "Commis pâtisserie / boulangerie" },
                            }
                        }
                    }
                },
            });

            await CountriesMigrator.Migrate(dbContext);
            await CountriesMigrator.MigrateActiveCountries(dbContext);
            await CitiesMigrator.Migrate(dbContext);
            await CitiesMigrator.MigrateActiveCities(dbContext);
            await LanguagesMigrator.Migrate(dbContext);
            await RegionsMigrator.Migrate(dbContext);

            //TODO: Uncomment when solution will use SQLite
            //await JobCategoriesMigrator.Migrate(dbContext);
            //await ProficienciesMigrator.Migrate(dbContext);

            dbContext.SaveChanges();
        }
    }
}

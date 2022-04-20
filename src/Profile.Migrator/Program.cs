using CommandLine;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Profile.Core;

namespace Profile.Migrator
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(MigrateDb);
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return -1;
            }
        }

        private static async Task MigrateDb(Options options)
        {
            var provider = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var dbOptions = new DbContextOptionsBuilder<ProfileDbContext>()
                .UseSqlServer(provider.GetConnectionString("ProfileDb"))
                .Options;

            await using var dbContext = new ProfileDbContext(dbOptions, new MigrationUserContext());

            foreach (var migration in await dbContext.Database.GetPendingMigrationsAsync())
            {
                Console.WriteLine("Pending migration: {0}", migration);
            }

            Console.WriteLine("Applying pending migrations...");
            await dbContext.Database.MigrateAsync();
            Console.WriteLine("Migrations are successfully applied");

            if (options.SeedDb)
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync();

                await CountriesMigrator.Migrate(dbContext);
                await CountriesMigrator.MigrateActiveCountries(dbContext);

                await CitiesMigrator.Migrate(dbContext);
                await CitiesMigrator.MigrateActiveCities(dbContext);
                await JobCategoriesMigrator.Migrate(dbContext);

                await ProficienciesMigrator.Migrate(dbContext);

                await LanguagesMigrator.Migrate(dbContext);

                await RegionsMigrator.Migrate(dbContext);

                await transaction.CommitAsync();

                Console.WriteLine("Database seed operation is completed");
            }
        }
    }
}
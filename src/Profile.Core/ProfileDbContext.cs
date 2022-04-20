using Microsoft.EntityFrameworkCore;
using Profile.Core.Extra;
using Profile.Core.Extra.User;
using Profile.Core.Languages;
using Profile.Core.Locations;
using Profile.Core.Proficiencies;
using Profile.Core.SharedKernel;
using Profile.Core.Users;

namespace Profile.Core
{
    public class ProfileDbContext : DbContext
    {
        private readonly IProfileUserContext userContext;
        public ProfileDbContext(DbContextOptions<ProfileDbContext> options, IProfileUserContext userContext) : base(options)
        {
            this.userContext = userContext;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Proficiency> Proficiencies { get; set; }
        public DbSet<UserLanguageProficiency> UserLanguageProficiencies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ActiveCountry> ActiveCountries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ActiveCity> ActiveCities { get; set; }
        public DbSet<UserExtraCity> UserExtraCities { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<JobRole> JobRoles { get; set; }
        public DbSet<UserExtraPreference> UserExtraPreferences { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<UserBirthData> UserBirthData { get; set; }
        public DbSet<UserCountryWorkPermit> UserCountryWorkPermits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfileDbContext).Assembly);

            // TODO: Applying query filter for IEntityTranslation interface
            // https://entityframeworkcore.com/knowledge-base/51763168/common-configurations-for-entities-implementing-an-interface
            modelBuilder.Entity<CountryTranslation>().HasQueryFilter(c => c.Culture == userContext.Culture);
            modelBuilder.Entity<CityTranslation>().HasQueryFilter(c => c.Culture == userContext.Culture);
            modelBuilder.Entity<ProficiencyTranslation>().HasQueryFilter(c => c.Culture == userContext.Culture);
            modelBuilder.Entity<JobRoleTranslation>().HasQueryFilter(c => c.Culture == userContext.Culture);
            modelBuilder.Entity<JobCategoryTranslation>().HasQueryFilter(c => c.Culture == userContext.Culture);
            modelBuilder.Entity<LanguageTranslation>().HasQueryFilter(c => c.Culture == userContext.Culture);

            base.OnModelCreating(modelBuilder);
        }
    }
}
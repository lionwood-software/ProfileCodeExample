using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Locations;

namespace Profile.Core.EntityConfigurations
{
    public class CountryTranslationConfiguration : IEntityTypeConfiguration<CountryTranslation>
    {
        public void Configure(EntityTypeBuilder<CountryTranslation> builder)
        {
            builder.ToTable("CountryTranslations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(256);
            builder.HasOne<Country>().WithMany(x => x.Translations).HasForeignKey(x => x.CountryId);

            builder.ConfigureTranslation();
        }
    }
}
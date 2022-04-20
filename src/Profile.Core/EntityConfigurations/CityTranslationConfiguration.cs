using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Locations;

namespace Profile.Core.EntityConfigurations
{
    public class CityTranslationConfiguration : IEntityTypeConfiguration<CityTranslation>
    {
        public void Configure(EntityTypeBuilder<CityTranslation> builder)
        {
            builder.ToTable("CityTranslations");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(256);
            builder.HasOne<City>().WithMany(x => x.Translations).HasForeignKey(x => x.CityId);

            builder.ConfigureTranslation();
        }
    }
}
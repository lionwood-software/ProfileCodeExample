using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Locations;

namespace Profile.Core.EntityConfigurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Timezone).HasMaxLength(256);

            builder.HasIndex(x => x.GeonamesId);
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.AsciiName);

            builder.HasOne(x => x.Country).WithMany().HasForeignKey(x => x.CountryId);
        }
    }
}
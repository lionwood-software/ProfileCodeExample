using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Locations;

namespace Profile.Core.EntityConfigurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Alpha2).HasMaxLength(2);
            builder.Property(x => x.Alpha3).HasMaxLength(3);

            builder.HasIndex(x => x.Alpha2).IsUnique();
            builder.HasIndex(x => x.Alpha3).IsUnique();
        }
    }
}
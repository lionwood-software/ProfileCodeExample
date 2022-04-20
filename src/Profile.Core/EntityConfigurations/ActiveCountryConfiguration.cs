using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Extra;

namespace Profile.Core.EntityConfigurations
{
    public class ActiveCountryConfiguration : IEntityTypeConfiguration<ActiveCountry>
    {
        public void Configure(EntityTypeBuilder<ActiveCountry> builder)
        {
            builder.ToTable("ActiveCountries");

            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Country).WithOne().HasForeignKey<ActiveCountry>(x => x.Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
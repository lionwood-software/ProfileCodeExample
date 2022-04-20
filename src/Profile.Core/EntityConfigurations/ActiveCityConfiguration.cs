using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Extra;

namespace Profile.Core.EntityConfigurations
{
    public class ActiveCityConfiguration : IEntityTypeConfiguration<ActiveCity>
    {
        public void Configure(EntityTypeBuilder<ActiveCity> builder)
        {
            builder.ToTable("ActiveCities");

            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.City).WithOne().HasForeignKey<ActiveCity>(x => x.Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
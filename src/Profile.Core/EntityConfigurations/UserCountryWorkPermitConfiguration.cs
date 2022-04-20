using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Extensions;
using Profile.Core.Users;

namespace Profile.Core.EntityConfigurations
{
    internal class UserCountryWorkPermitConfiguration : IEntityTypeConfiguration<UserCountryWorkPermit>
    {
        public void Configure(EntityTypeBuilder<UserCountryWorkPermit> builder)
        {
            builder.ToTable("UserCountryWorkPermits");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Country).WithMany();
            builder.ConfigureAudit();
        }
    }
}

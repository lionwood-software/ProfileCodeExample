using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Extensions;
using Profile.Core.Users;

namespace Profile.Core.EntityConfigurations
{
    internal class UserBirthDataConfiguration : IEntityTypeConfiguration<UserBirthData>
    {
        public void Configure(EntityTypeBuilder<UserBirthData> builder)
        {
            builder.ToTable("UserBirthData");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Country).WithMany();
            builder.HasOne(x => x.Region).WithMany();
            builder.ConfigureAudit();
        }
    }
}

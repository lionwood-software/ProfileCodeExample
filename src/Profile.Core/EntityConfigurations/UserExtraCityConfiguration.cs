using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Extensions;
using Profile.Core.Extra.User;

namespace Profile.Core.EntityConfigurations
{
    public class UserExtraCityConfiguration : IEntityTypeConfiguration<UserExtraCity>
    {
        public void Configure(EntityTypeBuilder<UserExtraCity> builder)
        {
            builder.ToTable("UserExtraCities");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.ConfigureAudit();

            builder.HasOne(x => x.City).WithMany().HasForeignKey(x => x.CityId);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        }
    }
}
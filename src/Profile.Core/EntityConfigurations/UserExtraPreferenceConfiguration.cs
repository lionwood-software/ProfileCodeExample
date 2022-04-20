using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Extensions;
using Profile.Core.Extra.User;

namespace Profile.Core.EntityConfigurations
{
    public class UserExtraPreferenceConfiguration : IEntityTypeConfiguration<UserExtraPreference>
    {
        public void Configure(EntityTypeBuilder<UserExtraPreference> builder)
        {
            builder.ToTable("UserExtraPreferences");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.ConfigureAudit();

            builder.HasIndex(x => new { x.UserId, x.JobCategoryId }).IsUnique();
        }
    }
}

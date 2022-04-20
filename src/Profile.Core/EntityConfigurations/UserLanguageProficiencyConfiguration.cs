using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Extensions;
using Profile.Core.Users;

namespace Profile.Core.EntityConfigurations
{
    internal class UserLanguageProficiencyConfiguration : IEntityTypeConfiguration<UserLanguageProficiency>
    {
        public void Configure(EntityTypeBuilder<UserLanguageProficiency> builder)
        {
            builder.ToTable("UserLanguageProficiencies");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.User).WithMany();
            builder.HasOne(x => x.Language).WithMany();
            builder.HasOne(x => x.Proficiency).WithMany();
            builder.ConfigureAudit();
            builder.HasIndex(x => new { x.UserId, x.LanguageCode }).IsUnique();
        }
    }
}

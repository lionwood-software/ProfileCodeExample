using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Extensions;
using Profile.Core.Users;

namespace Profile.Core.EntityConfigurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.FirstName).HasMaxLength(32);
            builder.Property(x => x.LastName).HasMaxLength(32);
            builder.Property(x => x.LanguageCode).HasMaxLength(20).IsRequired();
            builder.Property(x => x.IsOnboarded).HasDefaultValue(false);
            builder.Property(x => x.PhoneNumber).HasMaxLength(50);
            builder.Property(x => x.Nir).HasMaxLength(15).IsRequired(false);
            builder.ConfigureAudit();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
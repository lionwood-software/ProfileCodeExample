using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Languages;

namespace Profile.Core.EntityConfigurations
{
    internal class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable("Languages");
            builder.HasKey(x => x.Code);
            builder.Property(x => x.Code).ValueGeneratedNever();
            builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
        }
    }
}
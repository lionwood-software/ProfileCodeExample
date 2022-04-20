using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Languages;

namespace Profile.Core.EntityConfigurations
{
    public class LanguageTranslationConfiguration : IEntityTypeConfiguration<LanguageTranslation>
    {
        public void Configure(EntityTypeBuilder<LanguageTranslation> builder)
        {
            builder.ToTable("LanguageTranslations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(256);
            builder.HasOne<Language>().WithMany(x => x.Translations).HasForeignKey(x => x.LanguageCode);

            builder.ConfigureTranslation();
        }
    }
}
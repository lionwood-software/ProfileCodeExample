using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Proficiencies;

namespace Profile.Core.EntityConfigurations
{
    public class ProficiencyTranslationConfiguration : IEntityTypeConfiguration<ProficiencyTranslation>
    {
        public void Configure(EntityTypeBuilder<ProficiencyTranslation> builder)
        {
            builder.ToTable("ProficiencyTranslations");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(256);
            builder.HasOne<Proficiency>().WithMany(x => x.Translations).HasForeignKey(x => x.ProficiencyId);

            builder.ConfigureTranslation();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Extra;

namespace Profile.Core.EntityConfigurations
{
    public class JobCategoryTranslationConfiguration : IEntityTypeConfiguration<JobCategoryTranslation>
    {
        public void Configure(EntityTypeBuilder<JobCategoryTranslation> builder)
        {
            builder.ToTable("JobCategoryTranslations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(256);
            builder.HasOne<JobCategory>().WithMany(x => x.Translations).HasForeignKey(x => x.JobCategoryId);

            builder.ConfigureTranslation();
        }
    }
}

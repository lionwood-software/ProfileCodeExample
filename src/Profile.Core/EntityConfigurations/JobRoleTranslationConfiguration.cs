using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Extra;

namespace Profile.Core.EntityConfigurations
{
    public class JobRoleTranslationConfiguration : IEntityTypeConfiguration<JobRoleTranslation>
    {
        public void Configure(EntityTypeBuilder<JobRoleTranslation> builder)
        {
            builder.ToTable("JobRoleTranslations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(256);
            builder.HasOne<JobRole>().WithMany(x => x.Translations).HasForeignKey(x => x.JobRoleId);

            builder.ConfigureTranslation();
        }
    }
}

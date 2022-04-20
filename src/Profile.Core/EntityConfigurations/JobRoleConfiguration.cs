using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Extra;

namespace Profile.Core.EntityConfigurations
{
    public class JobRoleConfiguration : IEntityTypeConfiguration<JobRole>
    {
        public void Configure(EntityTypeBuilder<JobRole> builder)
        {
            builder.ToTable("JobRoles");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).HasMaxLength(256);

            builder.HasOne(x => x.JobCategory).WithMany(x => x.JobRoles).HasForeignKey(x => x.JobCategoryId);
        }
    }
}

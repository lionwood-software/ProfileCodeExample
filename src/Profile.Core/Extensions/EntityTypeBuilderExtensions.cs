using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.SharedKernel;

namespace Profile.Core.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder<T> ConfigureAudit<T>(this EntityTypeBuilder<T> builder)
            where T : class, IAuditable
        {
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.ModifiedBy).IsRequired();
            builder.Property(x => x.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.ModifiedDate).HasDefaultValueSql("GETUTCDATE()");
            return builder;
        }
    }
}

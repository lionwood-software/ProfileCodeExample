using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.SharedKernel;

namespace Profile.Core.EntityConfigurations
{
    public static class EntityTypeBuilderExtension
    {
        public static EntityTypeBuilder<TTranslatable> ConfigureTranslation<TTranslatable>(
            this EntityTypeBuilder<TTranslatable> builder) where TTranslatable : class, IEntityTranslation
        {
            builder.Property(x => x.Culture).HasMaxLength(32);
            builder.HasIndex(x => x.Culture);
            return builder;
        }
    }
}
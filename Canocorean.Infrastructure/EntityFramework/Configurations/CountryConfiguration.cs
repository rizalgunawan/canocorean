using Canocorean.Infrastructure.EntityFramework.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Canocorean.Infrastructure.EntityFramework.Configurations
{
    [UsedImplicitly]
    public sealed class CountryConfiguration: IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                new Country("RU"),
                new Country("US")
                );

            builder.HasKey(t => t.ISOCode);
            builder.Property(t => t.ISOCode).HasMaxLength(3);
        }
    }
}

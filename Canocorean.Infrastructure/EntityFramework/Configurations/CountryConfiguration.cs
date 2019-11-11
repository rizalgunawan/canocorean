using Canocorean.Infrastructure.EntityFramework.ReferenceEntities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Canocorean.Infrastructure.EntityFramework.Configurations
{
    [UsedImplicitly]
    public sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                new { ISOCode = "RU", Name = "Russian Federation" },
                new { ISOCode = "US", Name = "United States of America" },
                new { ISOCode = "CN", Name = "People's Republic of China" }
                );

            builder.HasKey(c => c.ISOCode);
            builder.Property(c => c.ISOCode).IsRequired().HasMaxLength(2);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(256);

            builder.HasMany(c => c.Provinces)
                .WithOne(p => p.Country)
                .HasForeignKey(p => p.CountryISOCode);
        }
    }
}

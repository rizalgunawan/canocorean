using Canocorean.Infrastructure.EntityFramework.ReferenceEntities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Canocorean.Infrastructure.EntityFramework.Configurations
{
    [UsedImplicitly]
    public sealed class ProvinceConfiguration : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.HasData(
                new { CountryISOCode = "RU", ISOCode = "MOS", Name = "Moskovskaya oblast'" },
                new { CountryISOCode = "RU", ISOCode = "LEN", Name = "Leningradskaya oblast'" },
                new { CountryISOCode = "RU", ISOCode = "NVS", Name = "Novosibirskaya oblast'" },
                new { CountryISOCode = "US", ISOCode = "NY", Name = "New York" },
                new { CountryISOCode = "US", ISOCode = "WA", Name = "Washington" },
                new { CountryISOCode = "US", ISOCode = "CA", Name = "California" },
                new { CountryISOCode = "CN", ISOCode = "HK", Name = "Hong Kong" },
                new { CountryISOCode = "CN", ISOCode = "HL", Name = "Harbin" },
                new { CountryISOCode = "CN", ISOCode = "JS", Name = "Nanjing" }
            );

            builder.HasKey(p =>
                new
                {
                    p.CountryISOCode,
                    p.ISOCode
                });
            builder.Property(p => p.CountryISOCode).IsRequired().HasMaxLength(2);
            builder.Property(p => p.ISOCode).IsRequired().HasMaxLength(3);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
        }
    }
}
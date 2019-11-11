using Canocorean.Users;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Canocorean.Infrastructure.EntityFramework.Configurations
{
    [UsedImplicitly]
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Login);
            builder.Property(u => u.Login).IsRequired().HasMaxLength(128);
            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
            builder.OwnsOne(t => t.Location, locationBuilder =>
            {
                locationBuilder.Property(l => l.CountryISOCode).IsRequired().HasMaxLength(2);
                locationBuilder.Property(l => l.ProvinceISOCode).IsRequired().HasMaxLength(3);
            });
        }
    }
}
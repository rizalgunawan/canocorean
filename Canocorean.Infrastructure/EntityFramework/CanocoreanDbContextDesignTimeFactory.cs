using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Canocorean.Infrastructure.EntityFramework
{
    [UsedImplicitly]
    public sealed class CanocoreanDbContextDesignTimeFactory : IDesignTimeDbContextFactory<CanocoreanDbContext>
    {
        private const string DefaultConnectionString =
            "Host=localhost;Port=5432;Database=Canocorean;Username=postgres;Password=example";
        public CanocoreanDbContext CreateDbContext(string[] args)
        {
            return new CanocoreanDbContext(new DbContextOptionsBuilder<CanocoreanDbContext>().UseNpgsql(DefaultConnectionString).Options);
        }
    }
}
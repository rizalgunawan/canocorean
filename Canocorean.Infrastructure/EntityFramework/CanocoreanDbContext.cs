using Microsoft.EntityFrameworkCore;

namespace Canocorean.Infrastructure.EntityFramework
{
    public class CanocoreanDbContext : DbContext
    {
        private const string DefaultConnectionString =
            "Host=localhost;Port=5432;Database=Canocorean;Username=postgres;Password=example";

        public CanocoreanDbContext(DbContextOptions<CanocoreanDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configuration for design time migrations
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(DefaultConnectionString);
            }
        }
    }
}

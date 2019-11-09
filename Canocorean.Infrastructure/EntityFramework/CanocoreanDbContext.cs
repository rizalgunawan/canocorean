using Microsoft.EntityFrameworkCore;

namespace Canocorean.Infrastructure.EntityFramework
{
    public class CanocoreanDbContext : DbContext
    {
        public CanocoreanDbContext()
        {

        }
        public CanocoreanDbContext(DbContextOptions<CanocoreanDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=postgres;Port=5432;Database=Canocorean;Username=postgres;Password=example");
            }
        }
    }
}

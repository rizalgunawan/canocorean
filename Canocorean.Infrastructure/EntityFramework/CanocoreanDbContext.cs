using Microsoft.EntityFrameworkCore;

namespace Canocorean.Infrastructure.EntityFramework
{
    public class CanocoreanDbContext : DbContext
    {
        public CanocoreanDbContext(DbContextOptions<CanocoreanDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using StarRepo.Data.Seed;
using StarRepo.Domain;

namespace StarRepo.Data
{
    /// <summary>
    /// Data access for the star repository.
    /// </summary>
    public class StarContext : DbContext
    {
        public StarContext(DbContextOptions<StarContext> opts)
            : base(opts) { }

        public DbSet<Observation> Observations { get; set; } = null!;
        public DbSet<Telescope> Telescopes { get; set; } = null!;
        public DbSet<Target> Targets { get; set; } = null!;        
    }
}
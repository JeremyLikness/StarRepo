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
        private readonly Seeder _seeder = null!;

        public StarContext(Seeder seeder) => _seeder = seeder;

        public StarContext(Seeder seeder, DbContextOptions<StarContext> opts)
            : base(opts) => _seeder = seeder;

        public DbSet<Observation> Observations { get; set; } = null!;
        public DbSet<Telescope> Telescopes { get; set; } = null!;
        public DbSet<Target> Targets { get; set; } = null!;

        public async Task CheckAndSeedAsync()
        {
            if (await Database.EnsureCreatedAsync())
            {
                await _seeder.SeedAsync(this);
            }
        }
    }
}
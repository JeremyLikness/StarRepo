using Microsoft.EntityFrameworkCore;
using StarRepo.Data;
using StarRepo.Domain;

namespace StarRepo.Server.GraphQL
{
    public sealed class QueryRepo : IAsyncDisposable
    {
        private readonly StarContext context;

        public QueryRepo(IDbContextFactory<StarContext> factory)
            => context = factory.CreateDbContext();

        public IQueryable<Telescope> GetTelescopes()
            => context.Telescopes;

        public IQueryable<Target> GetTargets()
            => context.Targets;

        public IQueryable<Observation> GetObservations()
            => context.Observations;


        public ValueTask DisposeAsync() => context.DisposeAsync();        
    }
}

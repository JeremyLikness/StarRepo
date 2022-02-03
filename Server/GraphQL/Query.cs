using StarRepo.Domain;

namespace StarRepo.Server.GraphQL
{
    public class Query
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Telescope> GetTelescopes([Service] QueryRepo queryRepo)
            => queryRepo.GetTelescopes();

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Target> GetTargets([Service] QueryRepo queryRepo)
            => queryRepo.GetTargets();

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Observation> GetObservations([Service] QueryRepo queryRepo)
            => queryRepo.GetObservations();
    }
}

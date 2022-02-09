using StarRepo.Domain;
using StarRepo.GraphQL;

namespace StarRepo.Client.GraphQL
{
    public class StarClientService
    {
        private readonly StarClient client = null!;

        public StarClientService(StarClient client)
            => this.client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<Telescope[]> GetTelescopesAsync()
        {
            var result = await client.GetTelescopes.ExecuteAsync();
            if (result == null || result.Data == null)
            {
                return Array.Empty<Telescope>();
            }
            return result.Data.Telescopes.Select(t =>
            new Telescope
            {
                Id = t.Id,
                Manufacturer = t.Manufacturer,
                Model = t.Model,
                ApertureMM = t.ApertureMM,
                FocalLengthMM = t.FocalLengthMM
            }).ToArray();
        }

        public async Task<Observation?> GetObservationAsync(Guid id)
        {
            var result = await client.GetObservations.ExecuteAsync(
                default,
                new ObservationFilterInput
                {
                    Id = new ComparableGuidOperationFilterInput
                    {
                        Eq = id
                    }
                });

            if (result == null || result.Data == null || !result.Data.Observations.Any())
            {
                return null;
            }

            return Transform(result.Data.Observations[0]);
        }

        public async Task<Observation[]> GetObservationsAsync(int sort, bool ascending = true,
            Guid? telescopeId = null)
        {
            var sortInput = new ObservationSortInput();
            ObservationSortInput[] sorts;
            var sortDirection = ascending ? SortEnumType.Asc : SortEnumType.Desc;
            switch (sort)
            {
                case 1:
                    sortInput.Target = new();
                    sortInput.Target.Name = sortDirection;
                    break;
                case 2:
                    sortInput.Telescope = new();
                    sortInput.Telescope.Manufacturer = sortDirection;
                    sortInput.Telescope.Model = sortDirection;
                    break;
                default:
                    break;
            }
            sorts = new[] { sortInput };
            ObservationFilterInput? observationFilter = default;
            if (telescopeId != null)
            {
                observationFilter = new ObservationFilterInput
                {
                    Telescope = new TelescopeFilterInput
                    {
                        Id = new ComparableGuidOperationFilterInput
                        {
                            Eq = telescopeId
                        }
                    }
                };
            }
            var result = await client.GetObservations.ExecuteAsync(sorts, observationFilter);

            if (result == null || result.Data == null)
            {
                return Array.Empty<Observation>();
            }

            var obs = result.Data.Observations.AsQueryable();
            if (sort == 0)
            {
                obs = ascending ? obs.OrderBy(o => o.ObservationDate)
                    : obs.OrderByDescending(o => o.ObservationDate);
            }

            return obs.Select(Transform).ToArray();
        }

        private static Observation Transform(IGetObservations_Observations ob) =>
            new()
            {
                Id = ob.Id,
                ObservationDate = ob.ObservationDate,
                Telescope = new Telescope
                {
                    Manufacturer = ob.Telescope!.Manufacturer,
                    Model = ob.Telescope!.Model,
                    FocalLengthMM = ob.Telescope!.FocalLengthMM
                },
                Target = new Target
                {
                    Name = ob.Target!.Name,
                    Description = ob.Target!.Description
                }
            };
    }
}

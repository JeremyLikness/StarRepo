using StarRepo.Client.Pages;
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
    }
}

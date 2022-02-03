using Microsoft.EntityFrameworkCore;
using StarRepo.Domain;

namespace StarRepo.Data.Seed
{
    public class Seeder
    {
        private readonly ImageHandler imageHandler = null!;

        public Seeder(ImageHandler imageHandler) =>
            this.imageHandler = imageHandler;

        public async Task SeedAsync(StarContext context)
        {
            context.Targets.AddRange(await ResourceHandler.ParseResourceAsync(
                t => new Target { Name = t[0], Description = t[1] }));
            context.Telescopes.AddRange(await ResourceHandler.ParseResourceAsync(
                t => new Telescope
                {
                    Manufacturer = t[0],
                    Model = t[1],
                    FocalLengthMM = int.Parse(t[2]),
                    ApertureMM = int.Parse(t[3]),
                }));
            await context.SaveChangesAsync();

            var root = Path.GetDirectoryName(typeof(Seeder).Assembly.Location);
            var observationProperties = await ResourceHandler.ParseResourceAsync(
                o => o, typeof(Observation));                
            foreach(string[] o in observationProperties)
            {
                var file = Path.Combine(root!, "Seed", o[0]);
                var date = DateTime.Parse(o[4]);
                var telescope = await context.Telescopes.Where(t =>
                    t.Manufacturer == o[1] &&
                    t.Model == o[2]).SingleAsync();
                var target = await context.Targets.Where(t =>
                    t.Name == o[3]).SingleAsync();
                var (fileId, ext) = imageHandler.ProcessForObservation(file);
                var observation = new Observation
                {
                    Target = target,
                    Telescope = telescope,
                    ObservationDate = date,
                    FileId = fileId,
                    Extension = ext
                };
                context.Observations.Add(observation);
            }

            await context.SaveChangesAsync();
        }
    }
}

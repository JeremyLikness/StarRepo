using StarRepo.Data;
using StarRepo.Domain;

namespace StarRepo.Server.GraphQL
{
    public class ObservationTypeExtensions : ObjectTypeExtension<Observation>
    {
        protected override void Configure(IObjectTypeDescriptor<Observation> descriptor)
        {
            descriptor
                .Field("image")
                .Type<StringType>()
                .Resolve(async ctx =>
                {
                    var handler = ctx.Service<ImageHandler>();
                    var observation = ctx.Parent<Observation>();
                    return await GetImageAsync(observation, handler, false);                    
                });

            descriptor
                .Field("thumbnail")
                .Type<StringType>()
                .Resolve(async ctx =>
                {
                    var handler = ctx.Service<ImageHandler>();
                    var observation = ctx.Parent<Observation>();
                    return await GetImageAsync(observation, handler, true);
                });
        }

        private static async Task<string> GetImageAsync(Observation obs, ImageHandler handler, bool isThumb)
        {
            if (obs.FileId == default || string.IsNullOrWhiteSpace(obs.Extension))
            {
                throw new Exception("'image' and 'thumbnail' require 'fileId' and 'extension' to also be included.");
            }

            var bytes = await handler.GetFileForObservationAsync(obs, isThumb);
            return $"data:image/jpeg;base64,{Convert.ToBase64String(bytes)}";
        }
    }
}

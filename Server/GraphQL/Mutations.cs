using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
using StarRepo.Data;
using StarRepo.Domain;

namespace StarRepo.Server.GraphQL
{
    public class Mutations
    {
        public async Task<TelescopeMutationResponse> ModifyTelescope(
            Telescope scope,
            [Service]IDbContextFactory<StarContext> factory,
            [Service] ITopicEventSender eventSender)
        {
            using var ctx = factory.CreateDbContext();
            var resp = new TelescopeMutationResponse();
            try
            {
                if (scope.Id == default)
                {
                    ctx.Telescopes.Add(scope);
                    resp.Message = "Successfully added the telescope.";
                }
                else
                {
                    ctx.Entry(scope).State = EntityState.Modified;
                    resp.Message = "Successfully modified the telescope.";
                }
                await ctx.SaveChangesAsync();
                resp.Id = scope.Id;
                resp.Success = true;
                await eventSender.SendAsync(SubscriptionsType.telescopeModified, scope);
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = $"An unexpected error occurred: {ex.Message}";
            }
            return resp;            
        }
    }
}

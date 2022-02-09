using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using StarRepo.Domain;

namespace StarRepo.Server.GraphQL
{
    public class SubscriptionsType : ObjectType
    {
        public const string telescopeModified = nameof(telescopeModified);

        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor
                .Field(telescopeModified)
                .Type<ObjectType<Telescope>>()
                .Resolve(context => context.GetEventMessage<Telescope>())
                .Subscribe(async context =>
                {
                    var receiver = context.Service<ITopicEventReceiver>();
                    ISourceStream stream =
                        await receiver.SubscribeAsync<string, Telescope>(telescopeModified);

                    return stream;
                });
        }
    }

}

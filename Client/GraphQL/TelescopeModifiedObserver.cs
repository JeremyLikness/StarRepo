using StarRepo.GraphQL;
using StrawberryShake;

namespace StarRepo.Client.GraphQL
{
    public sealed class TelescopeModifiedObserver : IDisposable, IObserver<IOperationResult<ITelescopeModifiedResult>>
    {
        private Action<ITelescopeModified_TelescopeModified>? callback;

        public TelescopeModifiedObserver(Action<ITelescopeModified_TelescopeModified> cb)
            => callback = cb;

        public void Dispose() => callback = null;
        
        public void OnCompleted() 
        {            
        }

        public void OnError(Exception error) => throw error;

        public void OnNext(IOperationResult<ITelescopeModifiedResult> value)
        {
            if (callback != null && value != null && value.Data != null)
            {
                callback(value.Data.TelescopeModified!);
            }
        }
    }
}

using System;
using System.Threading.Tasks;

namespace MyFirstEventStore.Interfaces.EventStore
{
    public interface IVersionRelevantEventStore<T> : IEventStore<T> where T: IAggregate
    {
        Task MakeSnapshot(Guid id, ulong version);
        Task<T> Project(Guid id, ulong version);
    }
}
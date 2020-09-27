using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstEventStore.Interfaces.EventStore
{
    public interface IEventStore<T> where T : IAggregate
    {
        Task AddEvent(IEvent<T> @event);
        Task MakeSnapshot(Guid id);
        Task<T> Project(Guid id);
        Task<IEnumerable<T>> ProjectAll();
    }
}
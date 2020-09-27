using System;

namespace MyFirstEventStore.Interfaces
{
    public interface IEvent<T> where T : IAggregate
    {
        public DateTime Published { get; }
        public Guid EntityId { get; }
        public ulong Version { get; }
    }
}
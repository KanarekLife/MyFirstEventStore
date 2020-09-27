using System;
using MyFirstEventStore.Interfaces;

namespace MyFirstEventStore.Implementations
{
    public class Snapshot<T> : IBaseEvent<T> where T : IAggregate
    {
        public Snapshot(T o, DateTime published)
        {
            Object = o;
            Published = published;
            EntityId = o.Id;
            Version = o.Version;
        }
        public DateTime Published { get; }
        public Guid EntityId { get; }
        public ulong Version { get; }
        public T Object { get; }
        T IBaseEvent<T>.Make()
        {
            return Object;
        }
    }
}
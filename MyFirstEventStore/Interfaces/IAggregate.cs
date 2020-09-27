using System;

namespace MyFirstEventStore.Interfaces
{
    public interface IAggregate
    {
        public Guid Id { get; set; }
        public ulong Version { get; set; }
    }
}
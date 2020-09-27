using System;
using System.Collections.Generic;
using MyFirstEventStore.Interfaces;

namespace MyFirstEventStoreTests.Aggregates
{
    public class Bulletin : IAggregate
    {
        public Guid Id { get; set; }
        public ulong Version { get; set; }
        public DateTime PublishTime { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
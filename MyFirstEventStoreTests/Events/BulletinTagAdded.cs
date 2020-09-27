using System;
using System.Linq;
using MyFirstEventStore.Interfaces;
using MyFirstEventStoreTests.Aggregates;

namespace MyFirstEventStoreTests.Events
{
    public class BulletinTagAdded : IUpdateEvent<Bulletin>
    {
        public BulletinTagAdded(string newTag, DateTime published, Guid entityId, ulong version)
        {
            NewTag = newTag;
            Published = published;
            EntityId = entityId;
            Version = version;
        }

        public DateTime Published { get; }
        public Guid EntityId { get; }
        public ulong Version { get; }
        public string NewTag { get; }

        Bulletin IUpdateEvent<Bulletin>.Apply(Bulletin aggregate)
        {
            aggregate.Tags = aggregate.Tags.Concat(new[] {NewTag});
            return aggregate;
        }
    }
}
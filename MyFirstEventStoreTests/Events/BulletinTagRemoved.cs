using System;
using System.Linq;
using MyFirstEventStore.Interfaces;
using MyFirstEventStoreTests.Aggregates;

namespace MyFirstEventStoreTests.Events
{
    public class BulletinTagRemoved : IUpdateEvent<Bulletin>
    {
        public BulletinTagRemoved(string tagToRemove, DateTime published, Guid entityId, ulong version)
        {
            TagToRemove = tagToRemove;
            Published = published;
            EntityId = entityId;
            Version = version;
        }

        public DateTime Published { get; }
        public Guid EntityId { get; }
        public ulong Version { get; }
        public string TagToRemove { get; }

        Bulletin IUpdateEvent<Bulletin>.Apply(Bulletin aggregate)
        {
            aggregate.Tags = aggregate.Tags.Where(x => x != TagToRemove);
            return aggregate;
        }
    }
}
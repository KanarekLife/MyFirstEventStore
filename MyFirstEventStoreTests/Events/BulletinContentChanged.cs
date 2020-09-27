using System;
using MyFirstEventStore.Interfaces;
using MyFirstEventStoreTests.Aggregates;

namespace MyFirstEventStoreTests.Events
{
    public class BulletinContentChanged : IUpdateEvent<Bulletin>
    {
        public BulletinContentChanged(string newContent, DateTime published, Guid entityId, ulong version)
        {
            NewContent = newContent;
            Published = published;
            EntityId = entityId;
            Version = version;
        }

        public DateTime Published { get; }
        public Guid EntityId { get; }
        public ulong Version { get; }
        public string NewContent { get; }

        Bulletin IUpdateEvent<Bulletin>.Apply(Bulletin aggregate)
        {
            aggregate.Content = NewContent;
            return aggregate;
        }
    }
}
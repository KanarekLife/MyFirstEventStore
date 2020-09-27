using System;
using MyFirstEventStore.Interfaces;
using MyFirstEventStoreTests.Aggregates;

namespace MyFirstEventStoreTests.Events
{
    public class BulletinTitleChanged : IUpdateEvent<Bulletin>
    {
        public BulletinTitleChanged(string newTitle, DateTime published, Guid entityId, ulong version)
        {
            NewTitle = newTitle;
            Published = published;
            EntityId = entityId;
            Version = version;
        }

        public DateTime Published { get; }
        public Guid EntityId { get; }
        public ulong Version { get; }
        public string NewTitle { get; }
        Bulletin IUpdateEvent<Bulletin>.Apply(Bulletin aggregate)
        {
            aggregate.Title = NewTitle;
            return aggregate;
        }
    }
}
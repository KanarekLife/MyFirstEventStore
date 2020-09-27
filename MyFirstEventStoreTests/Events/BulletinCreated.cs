using System;
using MyFirstEventStore.Interfaces;
using MyFirstEventStoreTests.Aggregates;

namespace MyFirstEventStoreTests.Events
{
    public class BulletinCreated : IBaseEvent<Bulletin>
    {
        public BulletinCreated(string title, string content, string[] tags, DateTime published, Guid entityId, ulong version = 0)
        {
            Title = title;
            Content = content;
            Tags = tags;
            Published = published;
            Version = version;
            EntityId = entityId;
        }

        public DateTime Published { get; }
        public Guid EntityId { get; }
        public ulong Version { get; }
        public string Title { get; }
        public string Content { get; }
        public string[] Tags { get; }

        Bulletin IBaseEvent<Bulletin>.Make()
        {
            return new Bulletin
            {
                Id = EntityId,
                Title = Title,
                Content = Content,
                PublishTime = Published,
                Tags = Tags
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MyFirstEventStore.Implementations;
using MyFirstEventStore.Interfaces;
using MyFirstEventStoreTests.Aggregates;
using MyFirstEventStoreTests.Events;
using Xunit;

namespace MyFirstEventStoreTests.EfMemoryEventStoreTests
{
    public class BasicTests
    {
        [Fact]
        public async Task DoesEventStoreWork()
        {
            var creationTime = DateTime.Now;
            var id = Guid.NewGuid();
            var events = new List<IEvent<Bulletin>>(new IEvent<Bulletin>[]
            {
                new BulletinCreated(
                    "SampleBulletin",
                    "Sample Content", 
                    Array.Empty<string>(),
                    creationTime,
                    id),
                new BulletinTitleChanged("TestBulletin", DateTime.Now, id, 1),
                new BulletinTagAdded("test", DateTime.Now, id, 2), 
                new BulletinTitleChanged("Sample", DateTime.Now, id, 3),
                new BulletinTagRemoved("test", DateTime.Now, id, 4), 
            });
            var eventStore = new EfEventStore<Bulletin>(new FakeDbContext());
            var expected = new Bulletin
            {
                Id = id,
                Content = "Sample Content",
                PublishTime = creationTime,
                Tags = ArraySegment<string>.Empty,
                Title = "Sample",
                Version = 4
            };

            foreach (var @event in events)
            {
                await eventStore.AddEvent(@event);
            }

            var result = await eventStore.Project(id);
            
            result.Should().BeEquivalentTo(expected);
        }
    }
}
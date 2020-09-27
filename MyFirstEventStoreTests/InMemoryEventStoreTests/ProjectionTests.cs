using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MyFirstEventStore.Implementations;
using MyFirstEventStore.Interfaces;
using MyFirstEventStoreTests.Aggregates;
using MyFirstEventStoreTests.Events;
using Xunit;

namespace MyFirstEventStoreTests.InMemoryEventStoreTests
{
    public class ProjectionTests
    {
        [Fact]
        public async Task Does_Projection_Work()
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
            });
            var eventStore = new InMemoryEventStore<Bulletin>();
            var afterSnapshotEvents = new List<IEvent<Bulletin>>(new IEvent<Bulletin>[]
            {
                new BulletinTitleChanged("Sample", DateTime.Now, id, 3),
                new BulletinTagRemoved("test", DateTime.Now, id, 4), 
            });
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

            await eventStore.MakeSnapshot(id);

            foreach (var @event in afterSnapshotEvents)
            {
                await eventStore.AddEvent(@event);
            }

            var result = await eventStore.Project(id);
            
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task Does_Projection_Work_2()
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
            var eventStore = new InMemoryEventStore<Bulletin>();
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

            await eventStore.MakeSnapshot(id, 3);

            var result = await eventStore.Project(id);
            
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task Does_Projection_Work_3()
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
            var eventStore = new InMemoryEventStore<Bulletin>();
            var expected = new Bulletin
            {
                Id = id,
                Content = "Sample Content",
                PublishTime = creationTime,
                Tags = new []{"test"},
                Title = "Sample",
                Version = 3
            };

            foreach (var @event in events)
            {
                await eventStore.AddEvent(@event);
            }

            var result = await eventStore.Project(id, 3);
            
            result.Should().BeEquivalentTo(expected);
        }
    }
}
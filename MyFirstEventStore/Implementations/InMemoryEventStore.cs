using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEventStore.Interfaces;
using MyFirstEventStore.Interfaces.EventStore;

namespace MyFirstEventStore.Implementations
{
    public class InMemoryEventStore<T> : IVersionRelevantEventStore<T> where T : IAggregate
    {
        private readonly List<IEvent<T>> _events = new List<IEvent<T>>();
        private readonly List<Snapshot<T>> _snapshots = new List<Snapshot<T>>();
        
        public Task AddEvent(IEvent<T> @event)
        {
            _events.Add(@event);
            return Task.CompletedTask;
        }

        public async Task MakeSnapshot(Guid id)
        {
            _snapshots.Add(new Snapshot<T>(await Project(id), DateTime.Now));
        }

        public Task<T> Project(Guid id)
        {
            var baseEvent = _events
                .Concat(_snapshots)
                .OfType<IBaseEvent<T>>()
                .Where(x => x.EntityId == id)
                .OrderBy(x => x.Version)
                .LastOrDefault();
            if (baseEvent == null)
            {
                throw new AggregateException("No base event found for given id.");
            }

            var entity = baseEvent.Make();

            var updateEvents = _events
                .OfType<IUpdateEvent<T>>()
                .Where(x => x.EntityId == id)
                .Where(x=>x.Version > entity.Version)
                .OrderBy(x => x.Version)
                .ToArray();

            var result = updateEvents.Aggregate(entity, (current, update) => current.Apply(update));

            return Task.FromResult(result);
        }

        public async Task<IEnumerable<T>> ProjectAll()
        {
            var guids = _events.Select(x => x.EntityId).Distinct();
            return await Task.WhenAll(guids.Select(Project));
        }

        public async Task MakeSnapshot(Guid id, ulong version)
        {
            _snapshots.Add(new Snapshot<T>(await Project(id, version), DateTime.Now));
        }

        public Task<T> Project(Guid id, ulong version)
        {
            var baseEvent = _events
                .Concat(_snapshots)
                .OfType<IBaseEvent<T>>()
                .Where(x => x.EntityId == id)
                .Where(x=>x.Version<=version)
                .OrderBy(x => x.Version)
                .LastOrDefault();
            if (baseEvent == null)
            {
                throw new AggregateException("No base event found for given id.");
            }

            var entity = baseEvent.Make();

            var updateEvents = _events
                .OfType<IUpdateEvent<T>>()
                .Where(x => x.EntityId == id)
                .Where(x=>x.Version > entity.Version)
                .Where(x=>x.Version <= version)
                .OrderBy(x => x.Version)
                .ToArray();

            var result = updateEvents.Aggregate(entity, (current, update) => current.Apply(update));

            return Task.FromResult(result);
        }
    }
}
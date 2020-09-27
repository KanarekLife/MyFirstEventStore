using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEventStore.Data;
using MyFirstEventStore.Interfaces;
using MyFirstEventStore.Interfaces.EventStore;

namespace MyFirstEventStore.Implementations
{
    public class EfEventStore<T> : IVersionRelevantEventStore<T> where T: IAggregate
    {
        private readonly IEventStoreDbContext _context;
        private readonly List<Snapshot<T>> _snapshots = new List<Snapshot<T>>();

        public EfEventStore(IEventStoreDbContext context)
        {
            _context = context;
        }

        public Task AddEvent(IEvent<T> @event)
        {
            _context.Events.Add(@event.ToEventDto());
            _context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public async Task MakeSnapshot(Guid id)
        {
            _snapshots.Add(new Snapshot<T>(await Project(id), DateTime.Now));
        }

        public Task<T> Project(Guid id)
        {
            var baseEvent = _context
                .Events
                .Where(x => x.EventType == EventType.Base)
                .Select(x => x.ToBaseEvent<T>())
                .ToArray()
                .Concat(_snapshots)
                .Where(x => x.EntityId == id)
                .OrderBy(x => x.Version)
                .LastOrDefault();
            
            if(baseEvent == null)
                throw new AggregateException("No base version found!");
            
            var updateEvents = _context
                .Events
                .Where(x => x.EventType == EventType.Update)
                .Where(x => x.Id == id)
                .Where(x => x.Version > baseEvent.Version)
                .Select(x=>x.ToUpdateEvent<T>());
            
            var entity = baseEvent.Make();
            foreach (var update in updateEvents)
            {
                entity = entity.Apply(update);
            }

            return Task.FromResult(entity);
        }

        public async Task<IEnumerable<T>> ProjectAll()
        {
            var guids = _context.Events.Select(x => x.Id).AsEnumerable();
            return await Task.WhenAll(guids.Select(Project));
        }

        public async Task MakeSnapshot(Guid id, ulong version)
        {
            _snapshots.Add(new Snapshot<T>(await Project(id, version), DateTime.Now));
        }

        public Task<T> Project(Guid id, ulong version)
        {
            var baseEvent = _context
                .Events
                .Where(x => x.EventType == EventType.Base)
                .Select(x=>x.ToBaseEvent<T>())
                .Concat(_snapshots)
                .Where(x => x.EntityId == id)
                .Where(x=>x.Version <= version)
                .OrderBy(x => x.Version)
                .LastOrDefault();
            
            if(baseEvent == null)
                throw new AggregateException("No base version found!");
            
            var updateEvents = _context
                .Events
                .Where(x => x.EventType == EventType.Update)
                .Where(x => x.Id == id)
                .Where(x => x.Version > baseEvent.Version)
                .Where(x=>x.Version <= version)
                .Select(x=>x.ToUpdateEvent<T>());
            
            var entity = baseEvent.Make();
            foreach (var update in updateEvents)
            {
                entity = entity.Apply(update);
            }

            return Task.FromResult(entity);
        }
    }
}
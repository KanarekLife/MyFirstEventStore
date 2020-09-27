using System;
using MyFirstEventStore.Data;
using MyFirstEventStore.Interfaces;
using Newtonsoft.Json;

namespace MyFirstEventStore
{
    public static class EventsExtensions
    {
        public static T Apply<T>(this T aggregator, IUpdateEvent<T> @event) where T : IAggregate
        {
            if (@event.Version > aggregator.Version)
            {
                var obj = @event.Apply(aggregator);
                obj.Version = @event.Version;
                return obj;
            }
            throw new AggregateException("Incorrect Version!");
        }
        
        public static EventDto ToEventDto<T>(this IEvent<T> @event) where T: IAggregate
        {
            var eventType = @event is IBaseEvent<T> ? EventType.Base : EventType.Update;
            var type = @event.GetType();
            var json = JsonConvert.SerializeObject(@event, type, Formatting.None, new JsonSerializerSettings());
            return new EventDto
            {
                Id = @event.EntityId,
                Version = @event.Version,
                PublishTime = @event.Published,
                EventType = eventType,
                TypeFullName = type.FullName,
                JsonContent = json
            };
        }
    }
}
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using MyFirstEventStore.Interfaces;
using Newtonsoft.Json;

namespace MyFirstEventStore.Data
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public ulong Version { get; set; }
        public DateTime PublishTime { get; set; }
        public EventType EventType { get; set; }
        public string TypeFullName { get; set; }
        public string JsonContent { get; set; }
        
        public IUpdateEvent<T> ToUpdateEvent<T>() where T : IAggregate
        {
            return (IUpdateEvent<T>) Deserialize();
        }
        
        public IBaseEvent<T> ToBaseEvent<T>() where T : IAggregate
        {
            return (IBaseEvent<T>) Deserialize();
        }
        
        private object Deserialize() => JsonConvert.DeserializeObject(JsonContent, EventTypes.FindEventType(TypeFullName));
    }
}
using System.Text.Json.Serialization;

namespace MyFirstEventStore.Data
{
        
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EventType
    {
        Update,
        Base
    }
}
namespace MyFirstEventStore.Interfaces
{
    public interface IUpdateEvent<T> : IEvent<T> where T : IAggregate
    {
        protected internal T Apply(T aggregate);
    }
}
namespace MyFirstEventStore.Interfaces
{
    public interface IBaseEvent<T> : IEvent<T> where T : IAggregate
    {
        protected internal T Make();
    }
}
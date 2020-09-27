using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFirstEventStore.Data;

namespace MyFirstEventStore.Interfaces
{
    public interface IEventStoreDbContext
    {
        public DbSet<EventDto> Events { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
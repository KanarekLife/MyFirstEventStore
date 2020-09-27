using Microsoft.EntityFrameworkCore;
using MyFirstEventStore.Data;
using MyFirstEventStore.Interfaces;

namespace MyFirstEventStoreTests.EfMemoryEventStoreTests
{
    public class FakeDbContext : DbContext, IEventStoreDbContext
    {
        public DbSet<EventDto> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("test");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventDto>(x =>
            {
                x.HasKey(y => new {y.Id, y.Version, y.PublishTime});
            });
        }
    }
}
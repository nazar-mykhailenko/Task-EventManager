using Microsoft.EntityFrameworkCore;
using ToDoListWithAuth.Models;

namespace ToDoListWithAuth.Data
{
    public class EventDBContext : DbContext
    {
        public EventDBContext(DbContextOptions<EventDBContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
    }
}

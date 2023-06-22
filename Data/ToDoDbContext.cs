using Microsoft.EntityFrameworkCore;
using ToDoListWithAuth.Models;

namespace ToDoListWithAuth.Data
{
	public class ToDoDbContext : DbContext
	{
		public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
		{
		}

		public DbSet<ToDoTask> Tasks { get; set; }
	}
}

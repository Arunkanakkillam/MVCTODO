using Microsoft.EntityFrameworkCore;
using Sample_.Models;

namespace Sample_.Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<TodoItem>todoItems { get; set; }
    }
}

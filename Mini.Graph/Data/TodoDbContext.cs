using Microsoft.EntityFrameworkCore;
using Mini.Graph.Model;

namespace Mini.Graph.Data;

public class TodoDbContext : DbContext
{
    public DbSet<Todo>? Todos { get; set; }
    protected TodoDbContext()
    {
    }

    public TodoDbContext(DbContextOptions options) : base(options)
    {
    }
}

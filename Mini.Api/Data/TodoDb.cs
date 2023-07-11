using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using Mini.Api.Model;

namespace Mini.Api.Data;

public class TodoDbContext : EFCoreStoreDbContext<TenantInfo>
{
    public DbSet<Todo>? Todos { get; set; }
     public TodoDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>().IsMultiTenant();
        base.OnModelCreating(modelBuilder);
    }
}
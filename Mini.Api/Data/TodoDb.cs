using System.Net;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using Mini.Api.Model;

namespace Mini.Api.Data;

public class TodoDbContext : EFCoreStoreDbContext<TenantInfo>
{
    public DbSet<Todo> Todos { get; set; }
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
        Todos = Set<Todo>() ?? throw new NullReferenceException(nameof(Todos));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Todo>().IsMultiTenant();
        modelBuilder.Entity<Todo>().HasKey(t => t.Id);
        modelBuilder.Entity<Todo>().ToTable("Todo");
        modelBuilder.Entity<TenantInfo>().ToTable("TenantInfo");

        base.OnModelCreating(modelBuilder);
    }
}
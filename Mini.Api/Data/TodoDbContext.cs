using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using Mini.Api.Model;
using Mini.Api.Service;

namespace Mini.Api.Data;

public class TodoDbContext : EFCoreStoreDbContext<TenantInfo>
{
    public DbSet<Todo>? Todos { get; set; }
    public ITenantService? Tenant { get; set; }

    public TodoDbContext(DbContextOptions options, ITenantService tenantService) : base(options)
    {
        Tenant = tenantService ?? throw new ArgumentNullException(nameof(tenantService));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>().HasKey(t => t.Id);
        modelBuilder.Entity<Todo>().ToTable("Todo");
        modelBuilder.Entity<Todo>().HasQueryFilter(t => t.TenantId == Tenant!.GetTenantId());
        modelBuilder.Entity<TenantInfo>().ToTable("TenantInfo");

        base.OnModelCreating(modelBuilder);
    }
}

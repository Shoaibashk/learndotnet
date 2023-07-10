using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using ProductWithTenant.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("InMemoryDb");
});

builder.Services.AddMultiTenant<TenantInfo>()
    .WithHeaderStrategy("tenant")
    .WithInMemoryStore(options =>
    {
        options.Tenants.Add(new TenantInfo { Id = "1", Identifier = "Apple" });
        options.Tenants.Add(new TenantInfo { Id = "0", Identifier = "Samsung" });
    });

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMultiTenant();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

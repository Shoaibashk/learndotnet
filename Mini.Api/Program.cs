using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using Mini.Api.Controller;
using Mini.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddMultiTenant<TenantInfo>().WithHeaderStrategy("tenant").WithEFCoreStore<TodoDbContext,TenantInfo>();

builder.Services.AddScoped<IMultiTenantStore<TenantInfo>, EFCoreStore<TodoDbContext,TenantInfo>>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMultiTenant();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapTodoApi();

app.Run();

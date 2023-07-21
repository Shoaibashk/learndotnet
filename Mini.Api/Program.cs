using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Mini.Api.Controller;
using Mini.Api.Data;
using Mini.Api.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ITenantService, TenantService>();

builder.Services.AddMultiTenant<TenantInfo>().WithHeaderStrategy("tenant").WithEFCoreStore<TodoDbContext, TenantInfo>();

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

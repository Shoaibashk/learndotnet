using Microsoft.EntityFrameworkCore;
using Mini.Graph;
using Mini.Graph.Actions;
using Mini.Graph.Actions.Query;
using Mini.Graph.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    options.EnableDetailedErrors();
});

builder.Services
    .AddGraphQLServer()
    .AddBuilder();

var app = builder.Build();

app.MapGraphQL();

app.Run();

using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini.Api.Data;
using Mini.Api.Model;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<TodoDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddMultiTenant<TenantInfo>().WithHeaderStrategy("tenant").WithEFCoreStore<TodoDbContext,TenantInfo>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// Add services to the container.
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

app.MapPost("/Tenant", async ([FromBody] TenantInfo tenant, IMultiTenantStore<TenantInfo> store) => await store.TryAddAsync(tenant));

app.MapGet("/", () => "Hello World!");

app.MapGet("/todoitems", async (TodoDbContext db) =>
    await db.Todos!.ToListAsync());

app.MapGet("/todoitems/complete", async (TodoDbContext db) =>
    await db.Todos!.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDbContext db) =>
    await db.Todos!.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/todoitems", async (Todo todo, TodoDbContext db) =>
{
    db.Todos!.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDbContext db) =>
{
    var todo = await db.Todos!.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDbContext db) =>
{
    if (await db.Todos!.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.NotFound();
});
app.MapPost("/upload", async (IFormFile file) =>
{
    string tempFile = CreateTempFilePath();
    using var stream = File.OpenWrite(tempFile);
    await file.CopyToAsync(stream);
    return tempFile;
    // dom more fancy stuff with the IFormFile
});
static string CreateTempFilePath()
{
    var filename = $"{Guid.NewGuid()}.tmp";
    var directoryPath = Path.Combine("temp", "uploads");
    if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

    return Path.Combine(directoryPath, filename);
}
/*app.MapTodosEndpoint();*/
/*
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();*/

app.Run();
/*
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}*/

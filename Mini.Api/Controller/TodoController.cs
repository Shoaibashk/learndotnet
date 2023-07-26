using System.Net;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini.Api.Data;
using Mini.Api.Model;
using Mini.Api.Service;

namespace Mini.Api.Controller;
public static class TodoController
{
    public static void MapTodoApi(this IEndpointRouteBuilder app)
    {
        app.MapPost("/Tenant", async ([FromBody] TenantInfo tenant, IMultiTenantStore<TenantInfo> store) => await store.TryAddAsync(tenant));

        app.MapGet("/Tenant", async (IMultiTenantStore<TenantInfo> store) => await store.GetAllAsync());

        app.MapGet("/todos", async (TodoDbContext db) =>
        {
            var todos = await db.Todos!.ToListAsync();
            return Results.Ok(todos);
        });

        app.MapGet("/todo/complete", async (TodoDbContext db) =>
            await db.Todos!.Where(t => t.IsComplete).ToListAsync());

        app.MapGet("/todo/{id}", async (int id, TodoDbContext db) =>
            await db.Todos!.FindAsync(id)
                is Todo todo
                    ? Results.Ok(todo)
                    : Results.NotFound());

        app.MapPost("/todo", async (Todo todo, TodoDbContext db) =>
        {
            db.Todos!.Add(todo);
            await db.SaveChangesAsync();

            return Results.Created($"/todo/{todo.Id}", todo);
        });

        app.MapPut("/todo/{id}", async (int id, Todo inputTodo, TodoDbContext db, CancellationToken cancellationToken) =>
        {
            // var tenantId = context.GetMultiTenantContext<TenantInfo>()?.TenantInfo!.Identifier;
            var todo = await db.Todos!.FirstOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (todo is null) return Results.NotFound();

            todo.Name = inputTodo.Name;
            todo.IsComplete = inputTodo.IsComplete;

            await db.SaveChangesAsync(cancellationToken);

            return Results.NoContent();
        });

        app.MapDelete("/todo/{id}", async (int id, TodoDbContext db) =>
        {
            if (await db.Todos!.FindAsync(id) is Todo todo)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
                return Results.Ok(todo);
            }

            return Results.NotFound();
        });
    }
}

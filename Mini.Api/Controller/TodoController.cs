using System.Net;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini.Api.Data;
using Mini.Api.Model;

namespace Mini.Api.Controller;
public static class TodoController
{
    public static void MapTodoApi(this IEndpointRouteBuilder app)
    {
        app.MapPost("/Tenant", async ([FromBody] TenantInfo tenant, IMultiTenantStore<TenantInfo> store) => await store.TryAddAsync(tenant));
        app.MapGet("/Tenant", async (IMultiTenantStore<TenantInfo> store) => await store.GetAllAsync());

        app.MapGet("/todos", async (TodoDbContext db, HttpContext context) =>
        {
            var tenantId = context.GetMultiTenantContext<TenantInfo>()?.TenantInfo!.Identifier;
            var ten = await db.TenantInfo!.ToListAsync();
            var todos = await db.Todos!.ToListAsync();
        });
        app.MapGet("/todo/complete", async (TodoDbContext db) =>
            await db.Todos!.Where(t => t.IsComplete).ToListAsync());

        app.MapGet("/todo/{id}", async (int id, TodoDbContext db) =>
            await db.Todos!.FindAsync(id)
                is Todo todo
                    ? Results.Ok(todo)
                    : Results.NotFound());

        app.MapPost("/todo", async (Todo todo, TodoDbContext db, HttpContext context) =>
        {
            todo.TenantId = (context.GetMultiTenantContext<TenantInfo>()?.TenantInfo!.Identifier);
            db.Todos!.Add(todo);
            await db.SaveChangesAsync();

            return Results.Created($"/todo/{todo.Id}", todo);
        });

        app.MapPut("/todo/{id}", async (int id, Todo inputTodo, TodoDbContext db) =>
        {
            var todo = await db.Todos!.FindAsync(id);

            if (todo is null) return Results.NotFound();

            todo.Name = inputTodo.Name;
            todo.IsComplete = inputTodo.IsComplete;

            await db.SaveChangesAsync();

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
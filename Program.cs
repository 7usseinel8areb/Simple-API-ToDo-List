using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRewriter(new RewriteOptions().AddRedirect("tasks/(.*)", "todos/$1"));


List<Todo> todos = new List<Todo>();

app.MapGet("/todos",()=>todos);

app.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound> (int id) =>
{
    Todo target = todos.SingleOrDefault(x => x.Id == id);
    return target is null ?
        TypedResults.NotFound() :
        TypedResults.Ok(target);
});

app.MapPost("/todos", (Todo MyTask) =>
{
    todos.Add(MyTask);
    return TypedResults.Created("/todos/{id}",MyTask);
});

app.MapDelete("/todos/{id}", (int id) =>
{
    todos.RemoveAll(todos => todos.Id == id);
    return TypedResults.NoContent();
});

app.Run();

public record Todo(int Id,string Name, DateTime DateTime, bool IsCompleted );

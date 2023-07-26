namespace Mini.Graph.Model;

public class Author
{
    public string? Name { get; set; }
}

public class Todo : EntityWithTenant
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public bool IsComplete { get; set; }
}

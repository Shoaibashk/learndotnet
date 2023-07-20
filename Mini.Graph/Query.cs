using Mini.Graph.Model;

namespace Mini.Graph;

public class Query
{
    public Book GetBook() =>
        new()
        {
            Title = "C# in depth.",
            Author = new Author
            {
                Name = "Jon Skeet"
            }
        };
}

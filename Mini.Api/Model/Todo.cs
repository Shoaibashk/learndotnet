using Finbuckle.MultiTenant;

namespace Mini.Api.Model
{
    [MultiTenant]
    public class Todo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public bool IsComplete { get; set; }
    }
}

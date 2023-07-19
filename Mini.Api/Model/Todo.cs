
namespace Mini.Api.Model
{
    public class Todo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public bool IsComplete { get; set; }
         public string? TenantId { get; set; }
    }
}

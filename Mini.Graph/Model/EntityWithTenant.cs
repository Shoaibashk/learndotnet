namespace Mini.Graph.Model;

public class EntityWithTenant
{
    public string? TenantId { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}

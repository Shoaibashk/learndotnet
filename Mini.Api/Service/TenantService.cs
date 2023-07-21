using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finbuckle.MultiTenant;

namespace Mini.Api.Service;

public interface ITenantService
{
    TenantInfo GetTenant();
    string GetTenantId();
    string GetTenantName();
}

public class TenantService : ITenantService
{
    private readonly ILogger<TenantService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantService(ILogger<TenantService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    public TenantInfo? GetTenant()
    {
        var tenantInfo = _httpContextAccessor?.HttpContext?.GetMultiTenantContext<TenantInfo>()?.TenantInfo;
        if (tenantInfo is null)
        {
            _logger.LogError("TenantInfo is null");
        }
        return tenantInfo;
    }

    public string GetTenantId() => GetTenant()!.Identifier?.ToString() ?? string.Empty;

    public string GetTenantName() => GetTenant()!.Name ?? string.Empty;
}

using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Persistence;

public class GetRolActualService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public GetRolActualService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public string GetRolActual()
    {
        ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        string? rol = user?.FindFirst(ClaimTypes.Role)?.Value;
        return string.IsNullOrWhiteSpace(rol) ? "Default" : rol;
    }
}

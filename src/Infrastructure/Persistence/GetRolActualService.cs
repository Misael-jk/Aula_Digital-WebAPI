using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Persistence
{
    public class GetRolActualService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetRolActualService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public string GetRolActual()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var rol = user?.FindFirst(ClaimTypes.Role)?.Value;
            return string.IsNullOrWhiteSpace(rol) ? "Default" : rol;
        }
    }
}

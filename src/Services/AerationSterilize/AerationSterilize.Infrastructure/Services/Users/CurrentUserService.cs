using System.Security.Claims;
using AerationSterilize.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace AerationSterilize.Infrastructure.Services.Users;
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    public Guid? UserId
    {
        get
        {
            var raw = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? User?.FindFirst("sub")?.Value;   // fallback cho JWT chuẩn OIDC

            return Guid.TryParse(raw, out var id) ? id : null;
        }
    }

    public Guid UserIdRequired =>
      UserId ?? throw new UnauthorizedAccessException("User is not authenticated.");

    public string UserName =>
        User?.FindFirst(ClaimTypes.Name)?.Value
        ?? User?.Identity?.Name
        ?? string.Empty;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;
}

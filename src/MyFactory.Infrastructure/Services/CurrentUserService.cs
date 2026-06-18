using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Infrastructure.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user is null)
                return Guid.Empty;

            // Try common claim types
            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? user.FindFirst("sub")?.Value;

            return Guid.TryParse(id, out var guid) ? guid : Guid.Empty;
        }
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
}

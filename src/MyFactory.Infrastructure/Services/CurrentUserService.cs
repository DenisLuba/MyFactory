using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Infrastructure.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    public Guid UserId => Guid.Empty;

    public bool IsAuthenticated => false;
}

using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Parties;

namespace MyFactory.Domain.Entities.Security;

public class UserEntity : ActivatableEntity
{
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public Guid RoleId { get; private set; }

    public IReadOnlyCollection<ContactLinkEntity> ContactLinks { get; private set; } = new List<ContactLinkEntity>();
    public IReadOnlyCollection<TokenEntity> Tokens { get; private set; } = new List<TokenEntity>();

    public UserEntity(string username, string passwordHash, Guid roleId)
    {
        Guard.AgainstNullOrWhiteSpace(username, nameof(username));
        Guard.AgainstNullOrWhiteSpace(passwordHash, nameof(passwordHash));
        Guard.AgainstEmptyGuid(roleId, nameof(roleId));

        Username = username;
        PasswordHash = passwordHash;
        RoleId = roleId;
    }

    public void ChangeRoleId(Guid roleId)
    {
        Guard.AgainstEmptyGuid(roleId, nameof(roleId));
        RoleId = roleId;
        Touch();
    }
}

public class RoleEntity : AuditableEntity
{
    public string Name { get; private set; }

    public RoleEntity(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }

    public void Rename(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Name = name;
        Touch();
    }
}

public class TokenEntity : AuditableEntity
{
    public Guid UserId { get; private set; }
    public string RefreshToken { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    public UserEntity? User { get; private set; }

    public TokenEntity(Guid userId, string refreshToken, DateTime expiresAt)
    {
        Guard.AgainstEmptyGuid(userId, nameof(userId));
        Guard.AgainstNullOrWhiteSpace(refreshToken, nameof(refreshToken));
        Guard.AgainstDefaultDate(expiresAt, nameof(expiresAt));

        UserId = userId;
        RefreshToken = refreshToken;
        ExpiresAt = expiresAt;
    }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool IsRevoked => RevokedAt.HasValue;

    public void Revoke(DateTime revokedAt)
    {
        Guard.AgainstDefaultDate(revokedAt, nameof(revokedAt));
        if (IsRevoked)
            return;

        RevokedAt = revokedAt;
        Touch();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;

namespace MyFactory.Domain.Entities.Identity;

/// <summary>
/// Aggregate root representing a security role that groups application permissions.
/// </summary>
public class Role : BaseEntity
{
    private readonly List<User> _users = new();

    private Role()
    {
    }

    public Role(string name)
    {
        Rename(name);
    }

    public string Name { get; private set; } = string.Empty;

    public IReadOnlyCollection<User> Users => _users.AsReadOnly();

    public void Rename(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Role name is required.");
        Name = name.Trim();
    }

    public void AttachUser(User user)
    {
        Guard.AgainstNull(user, nameof(user));
        if (user.RoleId != Id)
        {
            throw new DomainException("User role does not match this role.");
        }

        if (_users.Any(existing => existing.Id == user.Id))
        {
            return;
        }

        _users.Add(user);
    }
}

/// <summary>
/// Aggregate root representing a system user that can authenticate into the platform.
/// </summary>
public class User : BaseEntity
{
    private User()
    {
    }

    public User(string username, string email, string passwordHash, Guid roleId)
        : this(username, email, passwordHash, roleId, DateTime.UtcNow)
    {
    }

    public User(string username, string email, string passwordHash, Guid roleId, DateTime createdAt)
    {
        UpdateUsername(username);
        UpdateEmail(email);
        SetPasswordHash(passwordHash);
        ChangeRole(roleId);
        SetCreatedAt(createdAt);
        IsActive = true;
    }

    public string Username { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public Guid RoleId { get; private set; }

    public Role? Role { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public void UpdateUsername(string username)
    {
        Guard.AgainstNullOrWhiteSpace(username, "Username is required.");
        Username = username.Trim();
    }

    public void UpdateEmail(string email)
    {
        Guard.AgainstNullOrWhiteSpace(email, "Email is required.");
        Email = email.Trim();
    }

    public void SetPasswordHash(string passwordHash)
    {
        Guard.AgainstNullOrWhiteSpace(passwordHash, "Password hash is required.");
        PasswordHash = passwordHash;
    }

    public void ChangeRole(Guid roleId)
    {
        Guard.AgainstEmptyGuid(roleId, "Role id is required.");
        RoleId = roleId;
    }

    private void SetCreatedAt(DateTime createdAt)
    {
        Guard.AgainstDefaultDate(createdAt, "Created timestamp is required.");
        CreatedAt = createdAt;
    }

    public void Activate()
    {
        if (IsActive)
        {
            throw new DomainException("User is already active.");
        }

        IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new DomainException("User is already inactive.");
        }

        IsActive = false;
    }
}

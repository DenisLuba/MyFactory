using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MyFactory.Domain.Common;

namespace MyFactory.Domain.Entities.Identity;

/// <summary>
/// Aggregate root representing a security role that groups application permissions.
/// </summary>
public class Role : BaseEntity
{
    public const int NameMaxLength = 100;
    private readonly List<User> _users = new();

    private Role() { }

    private Role(string name, string? description)
    {
        Rename(name);
        UpdateDescription(description);
    }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public IReadOnlyCollection<User> Users => _users.AsReadOnly();

    public static Role Create(string name, string? description)
    {
        return new Role(name, description);
    }

    public void Rename(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Role name is required.");
        var trimmed = name.Trim();
        if (trimmed.Length > NameMaxLength)
        {
            throw new DomainException($"Role name cannot exceed {NameMaxLength} characters.");
        }
        Name = trimmed;
    }

    public void UpdateDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            Description = null;
            return;
        }

        Description = description.Trim();
    }

    public void AttachUser(User user)
    {
        Guard.AgainstNull(user, nameof(user));
        if (user.RoleId != Id)
        {
            throw new DomainException("User RoleId mismatch.");
        }
        if (user.Role != null && user.Role.Id != Id)
        {
            throw new DomainException("User.Role navigation mismatch.");
        }

        if (_users.Exists(u => u.Id == user.Id))
        {
            return;
        }

        _users.Add(user);
    }

    public void DetachUser(User user)
    {
        Guard.AgainstNull(user, nameof(user));
        var index = _users.FindIndex(u => u.Id == user.Id);
        if (index == -1)
        {
            return;
        }
        _users.RemoveAt(index);
    }
}

/// <summary>
/// Aggregate root representing a system user that can authenticate into the platform.
/// </summary>
public class User : BaseEntity
{
    public const int UsernameMaxLength = 100;

    private static readonly Regex EmailRegex = new(
        pattern: "^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$",
        options: RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private User() { }

    private User(string username, string email, string passwordHash, Guid roleId)
    {
        UpdateUsername(username);
        UpdateEmail(email);
        SetPasswordHash(passwordHash);
        ChangeRole(roleId);
        IsActive = true;
    }

    public string Username { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public Guid RoleId { get; private set; }

    public Role? Role { get; private set; }

    public bool IsActive { get; private set; }

    public static User Create(string username, string email, string passwordHash, Guid roleId)
    {
        return new User(username, email, passwordHash, roleId);
    }

    public void UpdateUsername(string username)
    {
        Guard.AgainstNullOrWhiteSpace(username, "Username is required.");
        var trimmed = username.Trim();
        if (trimmed.Length > UsernameMaxLength)
        {
            throw new DomainException($"Username cannot exceed {UsernameMaxLength} characters.");
        }
        Username = trimmed;
    }

    public void UpdateEmail(string email)
    {
        Guard.AgainstNullOrWhiteSpace(email, "Email is required.");
        var normalized = email.Trim().ToLowerInvariant();
        if (!EmailRegex.IsMatch(normalized))
        {
            throw new DomainException("Email format is invalid.");
        }
        Email = normalized;
    }

    public void SetPasswordHash(string passwordHash)
    {
        Guard.AgainstNullOrWhiteSpace(passwordHash, "Password hash is required.");
        var trimmed = passwordHash.Trim();
        if (trimmed.Length < 32)
        {
            throw new DomainException("Password hash must be at least 32 characters.");
        }
        PasswordHash = trimmed;
    }

    public void ChangeRole(Guid roleId)
    {
        Guard.AgainstEmptyGuid(roleId, "Role id is required.");
        RoleId = roleId;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new DomainException("User is already inactive.");
        }
        IsActive = false;
    }

    public void Activate()
    {
        if (IsActive)
        {
            throw new DomainException("User is already active.");
        }
        IsActive = true;
    }
}

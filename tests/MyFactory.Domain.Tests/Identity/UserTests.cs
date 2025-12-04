using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Identity;
using Xunit;

namespace MyFactory.Domain.Tests.Identity;

public class UserTests
{
    [Fact]
    public void Constructor_WithValidValues_SetsProperties()
    {
        var roleId = Guid.NewGuid();
        var createdAt = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc);

        var user = new User("john", "john@example.com", "hash", roleId, createdAt);

        Assert.Equal("john", user.Username);
        Assert.Equal("john@example.com", user.Email);
        Assert.Equal(roleId, user.RoleId);
        Assert.Equal(createdAt, user.CreatedAt);
        Assert.True(user.IsActive);
    }

    [Fact]
    public void Constructor_WithBlankUsername_ThrowsDomainException()
    {
        var roleId = Guid.NewGuid();

        Assert.Throws<DomainException>(() => new User(string.Empty, "john@example.com", "hash", roleId));
    }
}

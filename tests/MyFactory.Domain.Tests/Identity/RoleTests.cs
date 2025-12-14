using System;
using MyFactory.Domain.Entities.Identity;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Identity;

public class RoleTests
{
    [Fact]
    public void Rename_WithValidName_UpdatesValue()
    {
        var role = new Role("Operator");

        role.Rename("Supervisor");

        Assert.Equal("Supervisor", role.Name);
    }

    [Fact]
    public void Rename_WithEmptyName_ThrowsDomainException()
    {
        var role = new Role("Operator");

        Assert.Throws<DomainException>(() => role.Rename(string.Empty));
    }

    [Fact]
    public void Constructor_WithDescription_SetsCreatedAtAndDescription()
    {
        var utcNow = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc);

        var role = new Role("Auditor", "Handles compliance", utcNow);

        Assert.Equal("Handles compliance", role.Description);
        Assert.Equal(utcNow, role.CreatedAt);
    }

    [Fact]
    public void UpdateDescription_WithTooLongValue_Throws()
    {
        var role = new Role("Operator");
        var longText = new string('a', Role.DescriptionMaxLength + 1);

        Assert.Throws<DomainException>(() => role.UpdateDescription(longText));
    }

    [Fact]
    public void UpdateDescription_WithNull_ClearsValue()
    {
        var role = new Role("Operator", "Initial");

        role.UpdateDescription(null);

        Assert.Null(role.Description);
    }
}

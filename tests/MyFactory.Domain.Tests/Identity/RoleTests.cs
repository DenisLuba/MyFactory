using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Identity;
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
}

using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Materials;

public class MaterialTypeTests
{
    [Fact]
    public void Rename_WithValidValue_UpdatesName()
    {
        var type = new MaterialType("Fabrics");

        type.Rename("Accessories");

        Assert.Equal("Accessories", type.Name);
    }

    [Fact]
    public void Constructor_WithEmptyName_Throws()
    {
        Assert.Throws<DomainException>(() => new MaterialType("   "));
    }
}

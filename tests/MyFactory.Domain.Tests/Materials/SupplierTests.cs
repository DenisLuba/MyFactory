using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Materials;
using Xunit;

namespace MyFactory.Domain.Tests.Materials;

public class SupplierTests
{
    [Fact]
    public void Deactivate_SetsSupplierInactive()
    {
        var supplier = new Supplier("Global Textiles", "contact@global.test");

        supplier.Deactivate();

        Assert.False(supplier.IsActive);
    }

    [Fact]
    public void Deactivate_WhenAlreadyInactive_Throws()
    {
        var supplier = new Supplier("Global Textiles", "contact@global.test");
        supplier.Deactivate();

        Assert.Throws<DomainException>(supplier.Deactivate);
    }
}

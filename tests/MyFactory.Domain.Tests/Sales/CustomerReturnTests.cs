using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Sales;
using Xunit;

namespace MyFactory.Domain.Tests.Sales;

public class CustomerReturnTests
{
    [Fact]
    public void AddItemAndApprove_Succeeds()
    {
        var customerReturn = new CustomerReturn("RT-001", Guid.NewGuid(), new DateOnly(2025, 3, 1), "Damaged");
        customerReturn.AddItem(Guid.NewGuid(), 2, "Scrap");

        customerReturn.Approve();

        Assert.Equal(ReturnStatuses.Approved, customerReturn.Status);
    }

    [Fact]
    public void ApproveWithoutItems_Throws()
    {
        var customerReturn = new CustomerReturn("RT-002", Guid.NewGuid(), new DateOnly(2025, 3, 1), "Damaged");

        Assert.Throws<DomainException>(() => customerReturn.Approve());
    }

    [Fact]
    public void Complete_AfterApprove_SetsCompleted()
    {
        var customerReturn = new CustomerReturn("RT-003", Guid.NewGuid(), new DateOnly(2025, 3, 1), "Damaged");
        customerReturn.AddItem(Guid.NewGuid(), 1, "Scrap");
        customerReturn.Approve();

        customerReturn.Complete();

        Assert.Equal(ReturnStatuses.Completed, customerReturn.Status);
    }

    [Fact]
    public void Complete_BeforeApprove_Throws()
    {
        var customerReturn = new CustomerReturn("RT-004", Guid.NewGuid(), new DateOnly(2025, 3, 1), "Damaged");
        customerReturn.AddItem(Guid.NewGuid(), 1, "Scrap");

        Assert.Throws<DomainException>(() => customerReturn.Complete());
    }

    [Fact]
    public void AddItem_WithDuplicateSpecification_Throws()
    {
        var customerReturn = new CustomerReturn("RT-005", Guid.NewGuid(), new DateOnly(2025, 3, 1), "Damaged");
        var specificationId = Guid.NewGuid();
        customerReturn.AddItem(specificationId, 1, "Scrap");

        Assert.Throws<DomainException>(() => customerReturn.AddItem(specificationId, 2, "Scrap"));
    }
}

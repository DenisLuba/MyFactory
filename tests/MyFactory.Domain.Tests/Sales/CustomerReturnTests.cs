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
        var customerReturn = new CustomerReturn("RT-001", Guid.NewGuid(), new DateTime(2025, 3, 1), "Damaged");
        customerReturn.AddItem(Guid.NewGuid(), 2, "Scrap");

        customerReturn.Approve();

        Assert.Equal(ReturnStatus.Approved, customerReturn.Status);
    }

    [Fact]
    public void ApproveWithoutItems_Throws()
    {
        var customerReturn = new CustomerReturn("RT-002", Guid.NewGuid(), new DateTime(2025, 3, 1), "Damaged");

        Assert.Throws<DomainException>(() => customerReturn.Approve());
    }
}

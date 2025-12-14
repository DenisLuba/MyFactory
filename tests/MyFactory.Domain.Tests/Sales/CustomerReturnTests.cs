using System;
using MyFactory.Domain.Entities.Sales;
using MyFactory.Domain.Enums;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Sales;

public class CustomerReturnTests
{
    [Fact]
    public void AddItemAndReceive_Succeeds()
    {
        var customerReturn = new CustomerReturn("RT-001", Guid.NewGuid(), new DateOnly(2025, 3, 1), "Damaged");
        customerReturn.AddItem(Guid.NewGuid(), 2, "Scrap");

        customerReturn.MarkAsReceived();

        Assert.Equal(ReturnStatus.Received, customerReturn.Status);
    }

    [Fact]
    public void ReceiveWithoutItems_Throws()
    {
        var customerReturn = new CustomerReturn("RT-002", Guid.NewGuid(), new DateOnly(2025, 3, 1), "Damaged");

        Assert.Throws<DomainException>(() => customerReturn.MarkAsReceived());
    }

    [Fact]
    public void Process_AfterReceived_SetsProcessed()
    {
        var customerReturn = new CustomerReturn("RT-003", Guid.NewGuid(), new DateOnly(2025, 3, 1), "Damaged");
        customerReturn.AddItem(Guid.NewGuid(), 1, "Scrap");
        customerReturn.MarkAsReceived();

        customerReturn.ProcessReturn();

        Assert.Equal(ReturnStatus.Processed, customerReturn.Status);
    }

    [Fact]
    public void Process_BeforeReceived_Throws()
    {
        var customerReturn = new CustomerReturn("RT-004", Guid.NewGuid(), new DateOnly(2025, 3, 1), "Damaged");
        customerReturn.AddItem(Guid.NewGuid(), 1, "Scrap");

        Assert.Throws<DomainException>(() => customerReturn.ProcessReturn());
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

using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Warehousing;
using Xunit;

namespace MyFactory.Domain.Tests.Warehousing;

public class PurchaseRequestTests
{
    [Fact]
    public void SubmitAndApprove_Succeeds()
    {
        var request = new PurchaseRequest("PR-1", new DateTime(2025, 1, 10));
        request.AddItem(Guid.NewGuid(), 10);

        request.Submit();
        request.Approve();

        Assert.Equal(PurchaseRequestStatus.Approved, request.Status);
    }

    [Fact]
    public void ApproveWhileDraft_Throws()
    {
        var request = new PurchaseRequest("PR-2", new DateTime(2025, 1, 10));
        request.AddItem(Guid.NewGuid(), 10);

        Assert.Throws<DomainException>(() => request.Approve());
    }
}

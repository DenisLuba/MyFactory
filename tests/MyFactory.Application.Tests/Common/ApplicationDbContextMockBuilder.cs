using System.Threading;
using Moq;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Tests.Common;

internal static class ApplicationDbContextMockBuilder
{
    public static Mock<IApplicationDbContext> Create(TestApplicationDbContext context)
    {
        var mock = new Mock<IApplicationDbContext>();

        mock.SetupGet(db => db.Users).Returns(context.Users);
        mock.SetupGet(db => db.Roles).Returns(context.Roles);
        mock.SetupGet(db => db.Materials).Returns(context.Materials);
        mock.SetupGet(db => db.MaterialTypes).Returns(context.MaterialTypes);
        mock.SetupGet(db => db.Suppliers).Returns(context.Suppliers);
        mock.SetupGet(db => db.MaterialPriceHistoryEntries).Returns(context.MaterialPriceHistoryEntries);
        mock.SetupGet(db => db.FileResources).Returns(context.FileResources);
        mock.SetupGet(db => db.Warehouses).Returns(context.Warehouses);
        mock.SetupGet(db => db.InventoryItems).Returns(context.InventoryItems);
        mock.SetupGet(db => db.InventoryReceipts).Returns(context.InventoryReceipts);
        mock.SetupGet(db => db.InventoryReceiptItems).Returns(context.InventoryReceiptItems);
        mock.SetupGet(db => db.PurchaseRequests).Returns(context.PurchaseRequests);
        mock.SetupGet(db => db.PurchaseRequestItems).Returns(context.PurchaseRequestItems);
        mock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns((CancellationToken token) => context.SaveChangesAsync(token));

        return mock;
    }
}

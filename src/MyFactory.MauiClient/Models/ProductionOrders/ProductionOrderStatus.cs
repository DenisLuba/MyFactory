namespace MyFactory.MauiClient.Models.ProductionOrders;

public enum ProductionOrderStatus
{
    New = 0,
    MaterialIssued = 1,
    Cutting = 2,
    Sewing = 3,
    Packaging = 4,
    Finished = 5,
    Cancelled = 6
}

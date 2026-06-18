namespace MyFactory.MauiClient.Models.ProductionOrders;

public record ProductionOrderMaterialResponse(
    Guid MaterialId,
    string MaterialName,
    decimal RequiredQty,
    decimal AvailableQty,
    decimal MissingQty);

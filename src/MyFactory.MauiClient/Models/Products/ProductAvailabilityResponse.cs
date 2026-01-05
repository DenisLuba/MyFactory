namespace MyFactory.MauiClient.Models.Products;

public record ProductAvailabilityResponse(
    Guid WarehouseId,
    string WarehouseName,
    int AvailableQty);

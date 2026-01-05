namespace MyFactory.WebApi.Contracts.Products;

public record ProductAvailabilityResponse(
    Guid WarehouseId,
    string WarehouseName,
    int AvailableQty);

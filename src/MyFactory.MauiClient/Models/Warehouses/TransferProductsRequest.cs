namespace MyFactory.MauiClient.Models.Warehouses;

public sealed record TransferProductsRequest(
    Guid FromWarehouseId,
    Guid ToWarehouseId,
    IReadOnlyCollection<TransferProductItemRequest> Items);

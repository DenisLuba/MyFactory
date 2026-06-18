namespace MyFactory.MauiClient.Models.Warehouses;

public sealed record TransferMaterialsRequest(
    Guid FromWarehouseId,
    Guid ToWarehouseId,
    IReadOnlyCollection<TransferMaterialItemRequest> Items);

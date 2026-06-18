namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record TransferMaterialsRequest(
    Guid FromWarehouseId,
    Guid ToWarehouseId,
    IReadOnlyCollection<TransferMaterialItemRequest> Items);

namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record TransferProductsRequest(
    Guid FromWarehouseId,
    Guid ToWarehouseId,
    IReadOnlyCollection<TransferProductItemRequest> Items);

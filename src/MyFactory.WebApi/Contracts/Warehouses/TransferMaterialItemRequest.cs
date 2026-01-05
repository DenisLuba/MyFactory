namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record TransferMaterialItemRequest(Guid MaterialId, decimal Qty);

namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record TransferProductItemRequest(Guid ProductId, int Qty);

namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record TransferProductItemRequest(Guid ProductId, decimal QtyPerPackage, decimal? PackageCount = null);

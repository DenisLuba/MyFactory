namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record AddProductToWarehouseRequest(Guid ProductId, decimal QtyPerPackage, decimal? PackageCount = null);

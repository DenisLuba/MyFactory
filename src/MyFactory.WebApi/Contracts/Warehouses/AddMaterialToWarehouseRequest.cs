namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record AddMaterialToWarehouseRequest(Guid MaterialId, decimal QtyPerPackage, decimal? PackageCount = null);

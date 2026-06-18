namespace MyFactory.Application.DTOs.Warehouses;

public sealed record TransferMaterialItemDto(
    Guid MaterialId,
    decimal QtyPerPackage,
    decimal? PackageCount = null
);
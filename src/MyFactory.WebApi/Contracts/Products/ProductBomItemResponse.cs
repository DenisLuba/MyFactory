namespace MyFactory.WebApi.Contracts.Products;

public record ProductBomItemResponse(
    Guid MaterialId,
    string MaterialName,
    string? Unit,
    decimal QtyPerUnit,
    decimal LastUnitPrice,
    decimal TotalCost);

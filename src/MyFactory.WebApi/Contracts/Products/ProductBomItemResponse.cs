namespace MyFactory.WebApi.Contracts.Products;

public record ProductBomItemResponse(
    Guid MaterialId,
    string MaterialName,
    decimal QtyPerUnit,
    decimal LastUnitPrice,
    decimal TotalCost);

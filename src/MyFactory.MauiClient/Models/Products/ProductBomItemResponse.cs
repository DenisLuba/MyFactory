namespace MyFactory.MauiClient.Models.Products;

public record ProductBomItemResponse(
    Guid MaterialId,
    string MaterialName,
    string? Unit,
    decimal QtyPerUnit,
    decimal LastUnitPrice,
    decimal TotalCost);

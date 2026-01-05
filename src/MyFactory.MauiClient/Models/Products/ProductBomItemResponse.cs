namespace MyFactory.MauiClient.Models.Products;

public record ProductBomItemResponse(
    Guid MaterialId,
    string MaterialName,
    decimal QtyPerUnit,
    decimal LastUnitPrice,
    decimal TotalCost);

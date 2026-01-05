namespace MyFactory.MauiClient.Models.Materials;

public record MaterialListItemResponse(
    Guid Id,
    string MaterialType,
    string Name,
    decimal TotalQty,
    string UnitCode);

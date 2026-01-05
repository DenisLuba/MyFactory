namespace MyFactory.WebApi.Contracts.Materials;

public record MaterialListItemResponse(
    Guid Id,
    string MaterialType,
    string Name,
    decimal TotalQty,
    string UnitCode);

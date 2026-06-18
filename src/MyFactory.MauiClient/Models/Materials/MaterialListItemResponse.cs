using MyFactory.MauiClient.Models.Common;

namespace MyFactory.MauiClient.Models.Materials;

public record MaterialListItemResponse(
    Guid Id,
    string MaterialType,
    string Name,
    decimal TotalQty,
    string UnitCode) : ListItemResponse(Id, Name);

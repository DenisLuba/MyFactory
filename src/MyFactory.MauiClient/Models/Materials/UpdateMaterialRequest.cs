namespace MyFactory.MauiClient.Models.Materials;

public record UpdateMaterialRequest(
    string Name,
    Guid MaterialTypeId,
    Guid UnitId,
    string? Color);

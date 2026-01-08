namespace MyFactory.MauiClient.Models.Materials;

public sealed record CreateMaterialRequest(string Name, Guid MaterialTypeId, Guid UnitId, string? Color);

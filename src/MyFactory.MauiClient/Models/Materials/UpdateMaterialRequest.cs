namespace MyFactory.MauiClient.Models.Materials;

public record UpdateMaterialRequest(
    string Code,
    string Name,
    Guid MaterialTypeId,
    Units Unit,
    bool IsActive);

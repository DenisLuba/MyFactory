namespace MyFactory.MauiClient.Models.Materials;

public record CreateMaterialRequest(
    string Code,
    string Name,
    Guid MaterialTypeId,
    Units Unit,
    bool IsActive = true);

namespace MyFactory.WebApi.Contracts.Materials;

public record CreateMaterialRequest(
    string Code,
    string Name,
    Guid MaterialTypeId,
    Units Unit,
    bool IsActive = true
);


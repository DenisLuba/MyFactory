namespace MyFactory.WebApi.Contracts.Materials;

public record UpdateMaterialRequest(
    string Code,
    string Name,
    Guid MaterialTypeId,
    Units Unit,
    bool IsActive
);
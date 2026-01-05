namespace MyFactory.WebApi.Contracts.Materials;

public record UpdateMaterialRequest(
    string Name,
    Guid MaterialTypeId,
    Guid UnitId,
    string? Color);

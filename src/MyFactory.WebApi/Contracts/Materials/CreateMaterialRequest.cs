namespace MyFactory.WebApi.Contracts.Materials;

public record CreateMaterialRequest(
    string Name,
    Guid MaterialTypeId,
    Guid UnitId,
    string? Color);

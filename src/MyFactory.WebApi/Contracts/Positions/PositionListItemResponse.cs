namespace MyFactory.WebApi.Contracts.Positions;

public record PositionListItemResponse(
    Guid Id,
    string Name,
    string? Code,
    Guid DepartmentId,
    string DepartmentName,
    bool IsActive);

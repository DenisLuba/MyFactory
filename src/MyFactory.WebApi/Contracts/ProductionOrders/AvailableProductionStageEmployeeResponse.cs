namespace MyFactory.WebApi.Contracts.ProductionOrders;

public sealed record AvailableProductionStageEmployeeResponse(
    Guid Id,
    string FullName,
    string DepartmentName,
    string PositionName,
    bool IsActive);

namespace MyFactory.MauiClient.Models.ProductionOrders;

public record AvailableProductionStageEmployeeResponse(
    Guid Id,
    string FullName,
    string DepartmentName,
    string PositionName,
    bool IsActive);

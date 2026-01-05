namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record AddProductionStageEmployeeRequest(
    Guid EmployeeId,
    int QtyPlanned,
    int QtyCompleted,
    DateOnly Date,
    decimal? HoursWorked);

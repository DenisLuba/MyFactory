namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record UpdateProductionStageEmployeeRequest(
    Guid EmployeeId,
    int QtyPlanned,
    int Qty,
    DateOnly Date,
    decimal? HoursWorked);

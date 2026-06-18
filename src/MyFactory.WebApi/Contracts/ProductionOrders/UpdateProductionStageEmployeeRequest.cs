namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record UpdateProductionStageEmployeeRequest(
    Guid EmployeeId,
    decimal AssignedQty,
    decimal CompletedQty,
    DateOnly Date);

namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record AddProductionStageEmployeeRequest(
    Guid EmployeeId,
    decimal AssignedQty,
    decimal CompletedQty,
    DateOnly Date);

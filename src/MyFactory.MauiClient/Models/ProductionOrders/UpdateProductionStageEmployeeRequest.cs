namespace MyFactory.MauiClient.Models.ProductionOrders;

public record UpdateProductionStageEmployeeRequest(
    Guid EmployeeId,
    decimal AssignedQty,
    decimal CompletedQty,
    DateOnly Date);

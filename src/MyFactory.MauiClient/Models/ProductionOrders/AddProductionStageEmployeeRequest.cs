namespace MyFactory.MauiClient.Models.ProductionOrders;

public record AddProductionStageEmployeeRequest(
    Guid EmployeeId,
    decimal AssignedQty,
    decimal CompletedQty,
    DateOnly Date);

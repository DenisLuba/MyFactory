namespace MyFactory.MauiClient.Models.ProductionOrders;

public record RegisterSewingOperationRequest(
    Guid AssignmentId,
    Guid EmployeeId,
    decimal QtySewn,
    decimal HoursWorked,
    DateOnly OperationDate);

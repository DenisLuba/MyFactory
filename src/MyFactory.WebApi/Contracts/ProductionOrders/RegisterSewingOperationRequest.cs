namespace MyFactory.WebApi.Contracts.ProductionOrders;

public sealed record RegisterSewingOperationRequest(
    Guid AssignmentId,
    Guid EmployeeId,
    decimal QtySewn,
    decimal HoursWorked,
    DateOnly OperationDate);

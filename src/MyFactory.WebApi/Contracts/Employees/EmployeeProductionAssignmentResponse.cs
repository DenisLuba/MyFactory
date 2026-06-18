using MyFactory.Domain.Entities.Production;

namespace MyFactory.WebApi.Contracts.Employees;

public record EmployeeProductionAssignmentResponse(
    Guid ProductionOrderId,
    int ProductionOrderNumber,
    ProductionOrderStatus Stage,
    decimal QtyAssigned,
    decimal QtyCompleted);

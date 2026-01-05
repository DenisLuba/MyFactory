using MyFactory.Domain.Entities.Production;

namespace MyFactory.WebApi.Contracts.Employees;

public record EmployeeProductionAssignmentResponse(
    Guid ProductionOrderId,
    string ProductionOrderNumber,
    ProductionOrderStatus Stage,
    int QtyAssigned,
    int QtyCompleted);

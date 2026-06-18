using MyFactory.Domain.Entities.Production;

namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionStageAssignmentResponse(
    Guid AssignmentId,
    ProductionStage Stage,
    Guid EmployeeId,
    string EmployeeName,
    decimal? PlanPerHour,
    decimal AssignedQty,
    decimal CompletedQty,
    DateOnly WorkDate);

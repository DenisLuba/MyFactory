
namespace MyFactory.WebApi.Contracts.Production;

public record ProductionGetOrderResponse(
    Guid Id,
    Guid SpecificationId,
    int QtyOrdered,
    Allocation[] Allocations
)

public record Allocation(Guid WorkshopId, int QtyAllocated)
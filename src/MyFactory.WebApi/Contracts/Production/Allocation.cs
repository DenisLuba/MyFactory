namespace MyFactory.WebApi.Contracts.Production;

public record Allocation(
    Guid WorkshopId,
    int QtyAllocated
);


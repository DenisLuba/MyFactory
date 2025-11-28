namespace MyFactory.MauiClient.Models.Production;

public record Allocation(
    Guid WorkshopId,
    int QtyAllocated);

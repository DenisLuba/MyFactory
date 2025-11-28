namespace MyFactory.MauiClient.Models.Production;

public record ProductionGetOrderResponse(
    Guid Id,
    Guid SpecificationId,
    int QtyOrdered,
    Allocation[] Allocation);

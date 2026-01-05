using MyFactory.Domain.Entities.Production;

namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionStageSummaryResponse(
    ProductionOrderStatus Stage,
    int CompletedQty,
    int RemainingQty);

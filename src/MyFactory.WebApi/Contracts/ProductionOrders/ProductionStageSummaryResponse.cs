using MyFactory.Domain.Entities.Production;

namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionStageSummaryResponse(
    ProductionOrderStatus Stage,
    decimal CompletedQty,
    decimal RemainingQty);

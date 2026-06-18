using MyFactory.MauiClient.Models.ProductionOrders;

namespace MyFactory.MauiClient.Models.ProductionOrders;

public record ProductionStageSummaryResponse(
    ProductionOrderStatus Stage,
    decimal CompletedQty,
    decimal RemainingQty);

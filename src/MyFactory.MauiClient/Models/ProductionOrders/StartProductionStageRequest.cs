using MyFactory.MauiClient.Models.ProductionOrders;

namespace MyFactory.MauiClient.Models.ProductionOrders;

public record StartProductionStageRequest(ProductionOrderStatus TargetStatus);

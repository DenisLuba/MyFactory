using MyFactory.Domain.Entities.Production;

namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record StartProductionStageRequest(ProductionOrderStatus TargetStatus);

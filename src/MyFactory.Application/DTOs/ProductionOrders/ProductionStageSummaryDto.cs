using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.DTOs.ProductionOrders;

public sealed class ProductionStageSummaryDto
{
    public ProductionOrderStatus Stage { get; init; }
    public int CompletedQty { get; init; }
    public int RemainingQty { get; init; }
}

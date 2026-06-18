using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.DTOs.ProductionOrders;

public sealed class ProductionStageSummaryDto
{
    public ProductionOrderStatus Stage { get; init; }
    public decimal CompletedQty { get; init; }
    public decimal RemainingQty { get; init; }
}

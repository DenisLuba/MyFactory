namespace MyFactory.Application.DTOs.ProductionOrders;

public sealed class ProductionOrderMaterialDto
{
    public Guid MaterialId { get; init; }
    public string MaterialName { get; init; } = null!;
    public decimal RequiredQty { get; init; }
    public decimal AvailableQty { get; init; }
    public decimal MissingQty { get; init; }
}

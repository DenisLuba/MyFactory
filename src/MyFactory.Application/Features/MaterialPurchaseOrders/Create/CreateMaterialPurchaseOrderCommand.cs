using MediatR;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Create;

public sealed record CreateMaterialPurchaseOrderCommand : IRequest<Guid>
{
    public Guid SupplierId { get; init; }
    public DateTime OrderDate { get; init; }
}

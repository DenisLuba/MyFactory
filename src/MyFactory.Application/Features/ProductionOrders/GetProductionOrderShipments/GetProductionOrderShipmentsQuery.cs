using MediatR;
using MyFactory.Application.DTOs.ProductionOrders;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrderShipments;

public sealed record GetProductionOrderShipmentsQuery(Guid ProductionOrderId)
    : IRequest<IReadOnlyList<ProductionOrderShipmentDto>>;

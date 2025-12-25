using MediatR;
using MyFactory.Application.DTOs.SalesOrders;

namespace MyFactory.Application.Features.SalesOrders.GetSalesOrderShipments;

public sealed record GetSalesOrderShipmentsQuery(Guid SalesOrderId)
    : IRequest<IReadOnlyList<SalesOrderShipmentDto>>;

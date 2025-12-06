using System;
using MediatR;
using MyFactory.Application.DTOs.Production;

namespace MyFactory.Application.Features.Production.Commands.AllocateProductionOrder;

public sealed record AllocateProductionOrderCommand(
    Guid ProductionOrderId,
    Guid WorkshopId,
    decimal QtyAllocated) : IRequest<ProductionOrderDto>;

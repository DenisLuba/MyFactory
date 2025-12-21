using System;
using MediatR;
using MyFactory.Application.DTOs.Production;

namespace MyFactory.Application.OldFeatures.Production.Commands.AllocateProductionOrder;

public sealed record AllocateProductionOrderCommand(
    Guid ProductionOrderId,
    Guid WorkshopId,
    decimal QtyAllocated) : IRequest<ProductionOrderDto>;

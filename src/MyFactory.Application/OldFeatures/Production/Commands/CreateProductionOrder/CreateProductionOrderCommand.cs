using System;
using MediatR;
using MyFactory.Application.DTOs.Production;

namespace MyFactory.Application.OldFeatures.Production.Commands.CreateProductionOrder;

public sealed record CreateProductionOrderCommand(
    string OrderNumber,
    Guid SpecificationId,
    decimal QtyOrdered) : IRequest<ProductionOrderDto>;

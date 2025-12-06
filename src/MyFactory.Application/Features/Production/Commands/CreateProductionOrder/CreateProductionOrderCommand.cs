using System;
using MediatR;
using MyFactory.Application.DTOs.Production;

namespace MyFactory.Application.Features.Production.Commands.CreateProductionOrder;

public sealed record CreateProductionOrderCommand(
    string OrderNumber,
    Guid SpecificationId,
    decimal QtyOrdered) : IRequest<ProductionOrderDto>;

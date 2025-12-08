using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.FinishedGoods;

namespace MyFactory.Application.Features.FinishedGoods.Commands.CreateShipment;

public sealed record CreateShipmentCommand(
    string ShipmentNumber,
    Guid CustomerId,
    DateTime ShipmentDate,
    IReadOnlyCollection<CreateShipmentItemDto> Items) : IRequest<ShipmentDto>;

public sealed record CreateShipmentItemDto(Guid SpecificationId, decimal Quantity, decimal UnitPrice);

using System;
using MediatR;
using MyFactory.Application.DTOs.FinishedGoods;

namespace MyFactory.Application.Features.FinishedGoods.Commands.ConfirmShipmentPayment;

public sealed record ConfirmShipmentPaymentCommand(Guid ShipmentId) : IRequest<ShipmentDto>;

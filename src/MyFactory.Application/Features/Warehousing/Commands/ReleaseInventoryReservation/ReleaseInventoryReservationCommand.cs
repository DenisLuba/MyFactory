using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Commands.ReleaseInventoryReservation;

public sealed record ReleaseInventoryReservationCommand(Guid WarehouseId, Guid MaterialId, decimal Quantity) : IRequest<InventoryItemDto>;

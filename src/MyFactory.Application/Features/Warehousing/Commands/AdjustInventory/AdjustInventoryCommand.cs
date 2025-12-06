using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Commands.AdjustInventory;

public sealed record AdjustInventoryCommand(Guid WarehouseId, Guid MaterialId, decimal QuantityDelta, decimal? UnitPrice) : IRequest<InventoryItemDto>;

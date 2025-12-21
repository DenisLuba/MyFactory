using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.ReserveInventory;

public sealed record ReserveInventoryCommand(Guid WarehouseId, Guid MaterialId, decimal Quantity) : IRequest<InventoryItemDto>;

using System;
using MediatR;
using MyFactory.Application.DTOs.Inventory;

namespace MyFactory.Application.OldFeatures.Inventory.Queries.GetWarehouseMaterialInventory;

public sealed record GetWarehouseMaterialInventoryQuery(Guid WarehouseId, Guid MaterialId) : IRequest<InventoryWarehouseMaterialDto>;

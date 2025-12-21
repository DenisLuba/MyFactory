using System;
using MediatR;
using MyFactory.Application.DTOs.Inventory;

namespace MyFactory.Application.OldFeatures.Inventory.Queries.GetMaterialInventory;

public sealed record GetMaterialInventoryQuery(Guid MaterialId) : IRequest<InventoryMaterialSummaryDto>;

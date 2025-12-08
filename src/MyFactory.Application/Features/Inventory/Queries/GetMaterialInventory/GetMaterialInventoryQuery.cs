using System;
using MediatR;
using MyFactory.Application.DTOs.Inventory;

namespace MyFactory.Application.Features.Inventory.Queries.GetMaterialInventory;

public sealed record GetMaterialInventoryQuery(Guid MaterialId) : IRequest<InventoryMaterialSummaryDto>;

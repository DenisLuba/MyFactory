using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Inventory;

namespace MyFactory.Application.OldFeatures.Inventory.Queries.GetInventorySummary;

public sealed record GetInventorySummaryQuery() : IRequest<IReadOnlyCollection<InventoryMaterialSummaryDto>>;

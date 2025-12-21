using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Queries.GetInventoryReceipts;

public sealed record GetInventoryReceiptsQuery(Guid? SupplierId, string? Status) : IRequest<IReadOnlyCollection<InventoryReceiptDto>>;

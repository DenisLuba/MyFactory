using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Queries.GetInventoryReceipts;

public sealed record GetInventoryReceiptsQuery(Guid? SupplierId, InventoryReceiptStatus? Status) : IRequest<IReadOnlyCollection<InventoryReceiptDto>>;

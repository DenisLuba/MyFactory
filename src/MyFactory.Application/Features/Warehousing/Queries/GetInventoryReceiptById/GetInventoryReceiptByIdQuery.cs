using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Queries.GetInventoryReceiptById;

public sealed record GetInventoryReceiptByIdQuery(Guid Id) : IRequest<InventoryReceiptDto>;

using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Queries.GetPurchaseRequestById;

public sealed record GetPurchaseRequestByIdQuery(Guid Id) : IRequest<PurchaseRequestDto>;

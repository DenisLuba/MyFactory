using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Queries.GetPurchaseRequests;

public sealed record GetPurchaseRequestsQuery(PurchaseRequestStatus? Status) : IRequest<IReadOnlyCollection<PurchaseRequestDto>>;

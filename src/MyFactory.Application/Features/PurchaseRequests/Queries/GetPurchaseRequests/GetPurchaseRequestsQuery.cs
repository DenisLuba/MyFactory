using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.PurchaseRequests;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Features.PurchaseRequests.Queries.GetPurchaseRequests;

public sealed record GetPurchaseRequestsQuery(string? Status) : IRequest<IReadOnlyCollection<PurchaseRequestDto>>;

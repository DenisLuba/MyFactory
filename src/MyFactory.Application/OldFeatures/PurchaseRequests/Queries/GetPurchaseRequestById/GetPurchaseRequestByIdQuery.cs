using System;
using MediatR;
using MyFactory.Application.DTOs.PurchaseRequests;

namespace MyFactory.Application.OldFeatures.PurchaseRequests.Queries.GetPurchaseRequestById;

public sealed record GetPurchaseRequestByIdQuery(Guid Id) : IRequest<PurchaseRequestDto>;

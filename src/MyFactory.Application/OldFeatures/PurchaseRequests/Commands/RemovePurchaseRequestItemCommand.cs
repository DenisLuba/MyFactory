using System;
using MediatR;
using MyFactory.Application.DTOs.PurchaseRequests;

namespace MyFactory.Application.OldFeatures.PurchaseRequests.Commands;

public sealed record RemovePurchaseRequestItemCommand(
    Guid PurchaseRequestId,
    Guid PurchaseRequestItemId) : IRequest<PurchaseRequestDto>;

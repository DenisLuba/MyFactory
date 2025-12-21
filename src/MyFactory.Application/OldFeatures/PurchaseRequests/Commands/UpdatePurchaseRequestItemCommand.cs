using System;
using MediatR;
using MyFactory.Application.DTOs.PurchaseRequests;

namespace MyFactory.Application.OldFeatures.PurchaseRequests.Commands;

public sealed record UpdatePurchaseRequestItemCommand(
    Guid PurchaseRequestId,
    Guid PurchaseRequestItemId,
    Guid MaterialId,
    decimal Quantity) : IRequest<PurchaseRequestDto>;

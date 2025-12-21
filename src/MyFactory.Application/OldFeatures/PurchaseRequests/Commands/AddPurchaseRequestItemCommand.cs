using System;
using MediatR;
using MyFactory.Application.DTOs.PurchaseRequests;

namespace MyFactory.Application.OldFeatures.PurchaseRequests.Commands;

public sealed record AddPurchaseRequestItemCommand(Guid PurchaseRequestId, Guid MaterialId, decimal Quantity) : IRequest<PurchaseRequestDto>;

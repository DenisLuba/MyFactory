using System;
using MediatR;
using MyFactory.Application.DTOs.PurchaseRequests;

namespace MyFactory.Application.OldFeatures.PurchaseRequests.Commands;

public sealed record SubmitPurchaseRequestCommand(Guid PurchaseRequestId) : IRequest<PurchaseRequestDto>;

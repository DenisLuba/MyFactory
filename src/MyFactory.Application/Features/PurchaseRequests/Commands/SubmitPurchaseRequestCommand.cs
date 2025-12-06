using System;
using MediatR;
using MyFactory.Application.DTOs.PurchaseRequests;

namespace MyFactory.Application.Features.PurchaseRequests.Commands;

public sealed record SubmitPurchaseRequestCommand(Guid PurchaseRequestId) : IRequest<PurchaseRequestDto>;

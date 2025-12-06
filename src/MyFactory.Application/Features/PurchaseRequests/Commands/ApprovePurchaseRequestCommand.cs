using System;
using MediatR;
using MyFactory.Application.DTOs.PurchaseRequests;

namespace MyFactory.Application.Features.PurchaseRequests.Commands;

public sealed record ApprovePurchaseRequestCommand(Guid PurchaseRequestId) : IRequest<PurchaseRequestDto>;

using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Commands.RejectPurchaseRequest;

public sealed record RejectPurchaseRequestCommand(Guid PurchaseRequestId) : IRequest<PurchaseRequestDto>;

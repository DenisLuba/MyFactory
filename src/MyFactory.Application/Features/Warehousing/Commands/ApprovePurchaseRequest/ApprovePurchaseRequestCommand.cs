using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Commands.ApprovePurchaseRequest;

public sealed record ApprovePurchaseRequestCommand(Guid PurchaseRequestId) : IRequest<PurchaseRequestDto>;

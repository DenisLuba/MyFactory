using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Commands.SubmitPurchaseRequest;

public sealed record SubmitPurchaseRequestCommand(Guid PurchaseRequestId) : IRequest<PurchaseRequestDto>;

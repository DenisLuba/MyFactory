using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Commands.AddPurchaseRequestItem;

public sealed record AddPurchaseRequestItemCommand(Guid PurchaseRequestId, Guid MaterialId, decimal Quantity) : IRequest<PurchaseRequestDto>;

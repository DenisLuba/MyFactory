using System;
using MediatR;
using MyFactory.Application.DTOs.PurchaseRequests;

namespace MyFactory.Application.OldFeatures.PurchaseRequests.Commands;

public sealed record CreatePurchaseRequestCommand(string PrNumber, DateTime CreatedAt) : IRequest<PurchaseRequestDto>;

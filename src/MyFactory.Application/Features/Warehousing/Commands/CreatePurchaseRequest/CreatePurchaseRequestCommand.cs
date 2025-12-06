using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Commands.CreatePurchaseRequest;

public sealed record CreatePurchaseRequestCommand(string PrNumber, DateTime CreatedAt) : IRequest<PurchaseRequestDto>;

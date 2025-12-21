using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Queries.GetReturns;

public sealed record GetReturnsQuery(string? Status) : IRequest<IReadOnlyCollection<ReturnDto>>;

using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Application.Features.FinishedGoods.Queries.GetReturns;

public sealed record GetReturnsQuery(ReturnStatus? Status) : IRequest<IReadOnlyCollection<ReturnDto>>;

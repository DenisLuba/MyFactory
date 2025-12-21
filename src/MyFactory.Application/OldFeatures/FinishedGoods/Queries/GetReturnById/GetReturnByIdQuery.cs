using System;
using MediatR;
using MyFactory.Application.DTOs.FinishedGoods;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Queries.GetReturnById;

public sealed record GetReturnByIdQuery(Guid ReturnId) : IRequest<ReturnDto>;

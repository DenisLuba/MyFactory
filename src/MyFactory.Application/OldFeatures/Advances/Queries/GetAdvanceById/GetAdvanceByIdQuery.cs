using System;
using MediatR;
using MyFactory.Application.DTOs.Advances;

namespace MyFactory.Application.OldFeatures.Advances.Queries.GetAdvanceById;

public sealed record GetAdvanceByIdQuery(Guid AdvanceId) : IRequest<AdvanceDto>;

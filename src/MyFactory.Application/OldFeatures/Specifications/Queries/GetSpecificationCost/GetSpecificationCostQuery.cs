using System;
using MediatR;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.OldFeatures.Specifications.Queries.GetSpecificationCost;

public sealed record GetSpecificationCostQuery(Guid SpecificationId, DateOnly? AsOfDate) : IRequest<SpecificationCostDto>;

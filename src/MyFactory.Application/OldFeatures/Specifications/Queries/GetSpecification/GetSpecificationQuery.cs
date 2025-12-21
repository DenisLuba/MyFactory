using System;
using MediatR;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.OldFeatures.Specifications.Queries.GetSpecification;

public sealed record GetSpecificationQuery(Guid SpecificationId) : IRequest<SpecificationDetailsDto>;

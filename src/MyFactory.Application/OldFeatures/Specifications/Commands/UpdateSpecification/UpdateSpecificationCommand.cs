using System;
using MediatR;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.OldFeatures.Specifications.Commands.UpdateSpecification;

public sealed record UpdateSpecificationCommand(
    Guid SpecificationId,
    string Sku,
    string Name,
    decimal PlanPerHour,
    string? Description) : IRequest<SpecificationMutationResultDto>;

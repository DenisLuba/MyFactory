using MediatR;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.OldFeatures.Specifications.Commands.CreateSpecification;

public sealed record CreateSpecificationCommand(
    string Sku,
    string Name,
    decimal PlanPerHour,
    string? Description) : IRequest<SpecificationMutationResultDto>;

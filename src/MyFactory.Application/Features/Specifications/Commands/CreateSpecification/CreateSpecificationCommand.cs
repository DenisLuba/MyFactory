using MediatR;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.Features.Specifications.Commands.CreateSpecification;

public sealed record CreateSpecificationCommand(
    string Sku,
    string Name,
    decimal PlanPerHour,
    string? Description) : IRequest<SpecificationMutationResultDto>;

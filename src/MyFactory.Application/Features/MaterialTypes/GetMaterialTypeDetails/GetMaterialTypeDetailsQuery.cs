using MediatR;
using MyFactory.Application.DTOs.MaterialTypes;

namespace MyFactory.Application.Features.MaterialTypes.GetMaterialTypeDetails;

public sealed record GetMaterialTypeDetailsQuery(Guid MaterialTypeId)
    : IRequest<MaterialTypeDetailsDto>;

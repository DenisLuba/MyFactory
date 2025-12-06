using System;
using MediatR;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.Features.Specifications.Commands.DeleteBomItem;

public sealed record DeleteSpecificationBomItemCommand(
    Guid SpecificationId,
    Guid BomItemId) : IRequest<SpecificationDeleteBomItemResultDto>;

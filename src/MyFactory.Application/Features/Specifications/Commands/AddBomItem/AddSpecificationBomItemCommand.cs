using System;
using MediatR;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.Features.Specifications.Commands.AddBomItem;

public sealed record AddSpecificationBomItemCommand(
    Guid SpecificationId,
    Guid MaterialId,
    decimal Quantity,
    string Unit,
    decimal UnitCost) : IRequest<SpecificationBomItemResultDto>;

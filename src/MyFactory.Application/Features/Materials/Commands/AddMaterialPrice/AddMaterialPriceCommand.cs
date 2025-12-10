using System;
using MediatR;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.Commands.AddMaterialPrice;

public sealed record AddMaterialPriceCommand(
    Guid MaterialId,
    Guid SupplierId,
    decimal Price,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo,
    string DocRef) : IRequest<MaterialPriceHistoryDto>;

using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.OldFeatures.Materials.Queries.GetMaterialPriceHistory;

public sealed record GetMaterialPriceHistoryQuery(Guid MaterialId, Guid? SupplierId) : IRequest<IReadOnlyCollection<MaterialPriceHistoryDto>>;

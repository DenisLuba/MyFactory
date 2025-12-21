using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.OldFeatures.Specifications.Queries.GetSpecificationBom;

public sealed record GetSpecificationBomQuery(Guid SpecificationId) : IRequest<IReadOnlyCollection<SpecificationBomItemDto>>;

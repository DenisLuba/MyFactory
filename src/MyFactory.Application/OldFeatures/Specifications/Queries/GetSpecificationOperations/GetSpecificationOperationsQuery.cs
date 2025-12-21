using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.OldFeatures.Specifications.Queries.GetSpecificationOperations;

public sealed record GetSpecificationOperationsQuery(Guid SpecificationId) : IRequest<IReadOnlyCollection<SpecificationOperationItemDto>>;

using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.Features.Specifications.Queries.ListSpecifications;

public sealed record ListSpecificationsQuery : IRequest<IReadOnlyCollection<SpecificationListItemDto>>;

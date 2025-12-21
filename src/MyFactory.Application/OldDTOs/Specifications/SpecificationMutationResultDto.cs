using System;

namespace MyFactory.Application.OldDTOs.Specifications;

public sealed record SpecificationMutationResultDto(Guid SpecificationId, string Status);

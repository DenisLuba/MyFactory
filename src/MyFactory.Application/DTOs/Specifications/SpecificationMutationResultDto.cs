using System;

namespace MyFactory.Application.DTOs.Specifications;

public sealed record SpecificationMutationResultDto(Guid SpecificationId, string Status);

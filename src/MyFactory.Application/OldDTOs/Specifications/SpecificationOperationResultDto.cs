using System;

namespace MyFactory.Application.OldDTOs.Specifications;

public sealed record SpecificationOperationResultDto(
    Guid SpecificationId,
    SpecificationOperationItemDto Item,
    string Status);

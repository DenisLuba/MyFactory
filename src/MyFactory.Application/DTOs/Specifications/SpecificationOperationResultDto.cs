using System;

namespace MyFactory.Application.DTOs.Specifications;

public sealed record SpecificationOperationResultDto(
    Guid SpecificationId,
    SpecificationOperationItemDto Item,
    string Status);

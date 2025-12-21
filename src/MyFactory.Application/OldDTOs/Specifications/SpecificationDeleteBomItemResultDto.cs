using System;

namespace MyFactory.Application.OldDTOs.Specifications;

public sealed record SpecificationDeleteBomItemResultDto(
    Guid SpecificationId,
    Guid BomItemId,
    string Status);

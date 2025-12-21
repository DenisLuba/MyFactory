using System;

namespace MyFactory.Application.OldDTOs.Specifications;

public sealed record SpecificationBomItemResultDto(
    Guid SpecificationId,
    SpecificationBomItemDto Item,
    string Status);

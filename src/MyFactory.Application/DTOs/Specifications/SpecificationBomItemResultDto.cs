using System;

namespace MyFactory.Application.DTOs.Specifications;

public sealed record SpecificationBomItemResultDto(
    Guid SpecificationId,
    SpecificationBomItemDto Item,
    string Status);

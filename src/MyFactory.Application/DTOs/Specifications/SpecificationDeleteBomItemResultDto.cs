using System;

namespace MyFactory.Application.DTOs.Specifications;

public sealed record SpecificationDeleteBomItemResultDto(
    Guid SpecificationId,
    Guid BomItemId,
    string Status);

using System;

namespace MyFactory.WebApi.Contracts.Finance;

public record OverheadItemDto(
    Guid Id,
    DateTime Date,
    string Article,
    decimal Amount,
    string Comment,
    OverheadStatus Status
);

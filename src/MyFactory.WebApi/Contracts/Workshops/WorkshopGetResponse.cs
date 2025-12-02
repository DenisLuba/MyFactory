using System;

namespace MyFactory.WebApi.Contracts.Workshops;

public record WorkshopGetResponse(
    Guid Id,
    string Name,
    WorkshopType Type,
    WorkshopStatus Status
);

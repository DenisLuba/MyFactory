using System;

namespace MyFactory.WebApi.Contracts.Workshops;

public record WorkshopsListResponse(
    Guid Id,
    string Name,
    WorkshopType Type,
    WorkshopStatus Status
);

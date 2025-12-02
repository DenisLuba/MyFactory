using System;

namespace MyFactory.WebApi.Contracts.Workshops;

public record WorkshopCreateResponse(
    Guid Id,
    WorkshopStatus Status
);

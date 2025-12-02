using System;

namespace MyFactory.WebApi.Contracts.Workshops;

public record WorkshopUpdateResponse(
    Guid Id,
    WorkshopStatus Status
);

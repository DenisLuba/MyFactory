using System;

namespace MyFactory.MauiClient.Models.Workshops;

public record WorkshopUpdateResponse(
    Guid Id,
    WorkshopStatus Status
);

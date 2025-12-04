using System;

namespace MyFactory.MauiClient.Models.Workshops;

public record WorkshopCreateResponse(
    Guid Id,
    WorkshopStatus Status
);

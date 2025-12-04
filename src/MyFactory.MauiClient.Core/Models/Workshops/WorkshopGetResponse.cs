using System;

namespace MyFactory.MauiClient.Models.Workshops;

public record WorkshopGetResponse(
    Guid Id,
    string Name,
    WorkshopType Type,
    WorkshopStatus Status
);

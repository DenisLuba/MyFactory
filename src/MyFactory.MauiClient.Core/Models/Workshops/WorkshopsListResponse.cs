using System;

namespace MyFactory.MauiClient.Models.Workshops;

public record WorkshopsListResponse(
    Guid Id,
    string Name,
    WorkshopType Type,
    WorkshopStatus Status
);
